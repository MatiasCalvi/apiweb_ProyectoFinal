using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IQuerys;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDOfertas : IDaoBDOfertas
    {
        private readonly string connectionString;
        private IOfertaQuerys _ofertaQuerys;

        public DaoBDOfertas(IOptions<BDConfiguration> dbConfig, IOfertaQuerys ofertaQuerys)
        {
            connectionString = dbConfig.Value.ConnectionString;
            _ofertaQuerys = ofertaQuerys;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<OfertaSalida>> ObtenerOfertas()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            using var reader = await dbConnection.QueryMultipleAsync(
                _ofertaQuerys.procesoAlmObt, 
                commandType: CommandType.StoredProcedure
            );

            var ofertas = reader.Read<OfertaSalida, PublicacionSalida, OfertaSalida>(
                (oferta, publicacion) =>
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
                splitOn: "Public_ID"
            ).ToList();

            return ofertas.ToList();
        }

        public async Task<OfertaSalida> ObtenerOfertaPorID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                _ofertaQuerys.obtenerOfertaQuery,
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

            if (ofertaSalida == null)
            {
                return new OfertaSalida
                {
                    Oferta_ID = pId,
                    Oferta_ProdOfer = new List<PublicacionSalida>()
                };
            }

            return ofertaSalida;
        }

        public async Task<bool> VerificarAutoria(int ofertaID, int usuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int count = await dbConnection.ExecuteScalarAsync<int>(
                _ofertaQuerys.verificarCreador, 
                new { Public_UsuarioID = usuarioId, 
                Public_ID = ofertaID 
            });

            return count == 1;
        }


        public async Task<List<OfertaSalida>> ObtenerOfertasPorUsuarioID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                _ofertaQuerys.traerOfertasPorUsuarioID,
                map: (oferta, publicacion) =>
                {
                    oferta.Oferta_ProdOfer = new List<PublicacionSalida>();
                    oferta.Oferta_ProdOfer.Add(publicacion);
                    return oferta;
                },
                param: new { UsuarioID = pId },
                commandType: CommandType.StoredProcedure,
                splitOn: "Public_ID"
            );

            var ofertasSalida = result.GroupBy(o => o.Oferta_ID).Select(g =>
            {
                var oferta = g.First();
                oferta.Oferta_ProdOfer = g.Select(o => o.Oferta_ProdOfer.First()).ToList();
                return oferta;
            }).ToList();

            return ofertasSalida;
        }

        public async Task<OfertaSalida> CrearOferta(OfertaCreacion pOferta)
        {
            try
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
                    _ofertaQuerys.procesoAlmCrear,
                    parameters, 
                    commandType: CommandType.StoredProcedure);

                return ofertaSalida;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al crear una nueva oferta: {ex}");
            }
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
                _ofertaQuerys.procesoAlmEdit,
                parametros,
                commandType: CommandType.StoredProcedure
            );
           
            return filasAfectadas > 0;
        }

        public async Task<bool> VerificarOfertaEstado(int pOfertaID, int pEstadoID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var resultado = await dbConnection.QueryFirstOrDefaultAsync<int>(
                _ofertaQuerys.procesoAlmVEstado,
                new { 
                        OfertaID = pOfertaID, 
                        EstadoID = pEstadoID 
                },
                commandType: CommandType.StoredProcedure
            );

            return resultado == 1;
        }

        public async Task<bool> CambiarEstadoOferta(int pOfertaID, int pEstadoID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(
                _ofertaQuerys.procesoAlmEstado,
                new { OfertaID = pOfertaID, EstadoID = pEstadoID}
            );

            return filasAfectadas > 0;
        }

        public async Task<bool> DesasociarPublicaciones(int pOfertaID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(
                _ofertaQuerys.DesasociarPublicaciones,
                new { OfertaID = pOfertaID }
            );

            return filasAfectadas > 0;
        }

        public async Task<bool> EliminarOferta(int? pOfertaID, int? pUsuarioID)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                bool filas = await dbConnection.ExecuteScalarAsync<bool>(
                    _ofertaQuerys.procesoAlmElim,
                    new { OfertaID = pOfertaID, UsuarioID = pUsuarioID },
                    commandType: CommandType.StoredProcedure);

                return filas;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al eliminar la oferta y sus publicaciones: {ex.Message}");
            }
        }
    }
}






