using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Datos.Querys;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDOfertas : IDaoBDOfertas
    {
        private readonly string connectionString;

        public DaoBDOfertas(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<OfertaSalida>> ObtenerOfertas(DateTime pFechaActual)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            using var reader = await dbConnection.QueryMultipleAsync(
                OfertaQuerys.procesoAlmObt,
                new { FechaActual = pFechaActual },
                commandType: CommandType.StoredProcedure
            );

            var ofertasUnicas = new Dictionary<int, OfertaSalida>();

            reader.Read<OfertaSalida, PublicacionSalida, OfertaSalida>(
                (oferta, publicacion) =>
                {
                    if (!ofertasUnicas.TryGetValue(oferta.Oferta_ID, out var ofertasExistentes))
                    {
                        ofertasExistentes = oferta;
                        ofertasExistentes.Oferta_ProdOfer = new List<PublicacionSalida>();
                        ofertasUnicas[oferta.Oferta_ID] = ofertasExistentes;
                    }

                    if (publicacion != null)
                    {
                        ofertasExistentes.Oferta_ProdOfer.Add(publicacion);
                    }

                    return ofertasExistentes;
                },
                splitOn: "Public_ID"
            );

            return ofertasUnicas.Values.ToList();
        }

        public async Task<OfertaSalida> ObtenerOfertaPorID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                OfertaQuerys.obtenerOfertaQuery,
                map: (oferta, publicacion) =>
                {
                    if (oferta.Oferta_ProdOfer == null)
                    {
                        oferta.Oferta_ProdOfer = new List<PublicacionSalida>();
                    }

                    if (publicacion != null)
                    {
                        oferta.Oferta_ProdOfer.Add(publicacion);
                    }

                    return oferta;
                },
                param: new { Oferta_ID = pId },
                splitOn: "Public_ID"
            );

            var ofertaSalida = result.FirstOrDefault();

            if (ofertaSalida == null || ofertaSalida.Oferta_ID == 0)
            {
                return null;
            }

            return ofertaSalida;
        }


        public async Task<bool> VerificarAutoria(int pOfertaID, int pUsuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int verificar = await dbConnection.ExecuteScalarAsync<int>(
                OfertaQuerys.verificarCreador, 
                new { Public_UsuarioID = pUsuarioId, 
                Public_ID = pOfertaID
                });

            return verificar == 1;
        }

        public async Task<int> VerificarDescuento(int pPublicID) 
        {                                                                       
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            DateTime fechaActual = DateTime.Now;

            IDataReader reader = await dbConnection.ExecuteReaderAsync(
                OfertaQuerys.procesoVDescuento,
                new
                {
                    PublicID = pPublicID,
                    FechaActual = fechaActual,
                });

            int descuento = 0;

            if (reader.Read())
            {
                descuento = reader[0] as int? ?? 0;
            }

            reader.Close();
            return descuento;
        }

        public async Task<List<OfertaSalida>> ObtenerOfertasPorUsuarioID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                OfertaQuerys.traerOfertasPorUsuarioID,
                map: (oferta, publicacion) =>
                {
                    if (oferta.Oferta_ProdOfer == null)
                    {
                        oferta.Oferta_ProdOfer = new List<PublicacionSalida>();
                    }

                    if (publicacion != null && !oferta.Oferta_ProdOfer.Any(p => p.Public_ID == publicacion.Public_ID))
                    {
                        oferta.Oferta_ProdOfer.Add(publicacion);
                    }

                    return oferta;
                },
                param: new { UsuarioID = pId },
                commandType: CommandType.StoredProcedure,
                splitOn: "Public_ID"
            );

            var ofertasSalida = result.GroupBy(o => o.Oferta_ID).Select(g =>
            {
                var oferta = g.FirstOrDefault();
                if (oferta != null)
                {
                    oferta.Oferta_ProdOfer = g.Select(o => o.Oferta_ProdOfer.FirstOrDefault()).Where(p => p != null).ToList();
                }
                return oferta;
            }).Where(o => o != null).ToList();

            return ofertasSalida;
        }

        public async Task<OfertaSalida> CrearOferta(OfertaCreacion pOferta)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var parameters = new DynamicParameters();
            parameters.Add("pOferta_UsuarioID", pOferta.Oferta_UsuarioID, DbType.Int32);
            parameters.Add("pOferta_Nombre", pOferta.Oferta_Nombre, DbType.String);
            parameters.Add("pOferta_Descuento", pOferta.Oferta_Descuento, DbType.Int32);
            parameters.Add("pOferta_FInicio", pOferta.Oferta_FInicio, DbType.DateTime);
            parameters.Add("pOferta_FFin", pOferta.Oferta_FFin, DbType.DateTime);
            parameters.Add("pOferta_FCreacion", pOferta.Oferta_FCreacion, DbType.DateTime);
            parameters.Add("@Oferta_ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("pProductosIds", string.Join(",", pOferta.Oferta_ProdOfer), DbType.String);

            OfertaSalida ofertaSalida = await dbConnection.QueryFirstOrDefaultAsync<OfertaSalida>(
                OfertaQuerys.procesoAlmCrear,
                parameters, 
                commandType: CommandType.StoredProcedure);

            return ofertaSalida;
        }

        public async Task<bool> EditarOferta(int pId, OfertaModif pOfertaModif)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var parametros = new DynamicParameters();
           
            parametros.Add("pOfertaNombre", pOfertaModif.Oferta_Nombre);
            parametros.Add("pOfertaDescuento", pOfertaModif.Oferta_Descuento);
            parametros.Add("pOfertaFInicio", pOfertaModif.Oferta_FInicio);
            parametros.Add("pOfertaFFin", pOfertaModif.Oferta_FFin);
            parametros.Add("pProductosIds", string.Join(",", pOfertaModif.Oferta_ProdOfer?.Where(x => x.HasValue).Select(x => x.Value.ToString()) ?? Enumerable.Empty<string>()));
            parametros.Add("pOfertaID", pId);
            parametros.Add("pOfertaFModif", pOfertaModif.Oferta_FModif);
            
            int filasAfectadas = await dbConnection.ExecuteAsync(
                OfertaQuerys.procesoAlmEdit,
                parametros,
                commandType: CommandType.StoredProcedure
            );
           
            return filasAfectadas > 0;
        }

        public async Task<bool> DesasociarPublicaciones(int pOfertaID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(
                OfertaQuerys.desasociarPublicaciones,
                new { OfertaID = pOfertaID }
            );

            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarOferta(int? pOfertaID, int? pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            bool filas = await dbConnection.ExecuteScalarAsync<bool>(
                OfertaQuerys.procesoAlmElim,
                new { OfertaID = pOfertaID, UsuarioID = pUsuarioID },
                commandType: CommandType.StoredProcedure);

            return filas;
        }

        public async Task<bool> OfertaCancelar(int pUsuarioID, int? pOfertaID, DateTime pFecha)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            bool filas = await dbConnection.ExecuteScalarAsync<bool>(
                OfertaQuerys.procesoAlmCan,
                new { OfertaID = pOfertaID, UsuarioID = pUsuarioID, PFecha = pFecha },
                commandType: CommandType.StoredProcedure);

            return filas;
        }
    }
}






