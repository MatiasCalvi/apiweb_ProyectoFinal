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

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                _ofertaQuerys.obtenerOfertasQuery,
                map: (oferta, publicacion) =>
                {
                    oferta.Oferta_ProdOfer = new List<PublicacionSalida>();
                    oferta.Oferta_ProdOfer.Add(publicacion);
                    return oferta;
                },
                param: new { Oferta_Estado = 3 },
                splitOn: "Public_ID"
            );

            var ofertaSalida = result.GroupBy(o => o.Oferta_ID).Select(g =>
            {
                var oferta = g.First();
                oferta.Oferta_ProdOfer = g.Select(o => o.Oferta_ProdOfer.First()).ToList();
                return oferta;
            }).ToList();

            return ofertaSalida;
        }

        public async Task<OfertaSalida> ObtenerOfertaPorID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                _ofertaQuerys.obtenerOfertaQuery,
                map: (oferta, publicacion) =>
                {
                    oferta.Oferta_ProdOfer = new List<PublicacionSalida>();
                    oferta.Oferta_ProdOfer.Add(publicacion);
                    return oferta;
                },
                param: new { Oferta_ID = pId },
                splitOn: "Public_ID"
            );

            var ofertaSalida = result.GroupBy(o => o.Oferta_ID).Select(g =>
            {
                var oferta = g.First();
                oferta.Oferta_ProdOfer = g.Select(o => o.Oferta_ProdOfer.First()).ToList();
                return oferta;
            }).FirstOrDefault();

            return ofertaSalida;
        }

        public async Task<bool> VerificarAutoria(int ofertaID, int usuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int count = await dbConnection.ExecuteScalarAsync<int>(_ofertaQuerys.verificarCreador, new { Public_UsuarioID = usuarioId, Public_ID = ofertaID });

            return count == 1;
        }


        public async Task<List<OfertaSalida>> ObtenerOfertasPorUsuarioID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                _ofertaQuerys.obtenerfertasPorUsuarioIDQuery,
                map: (oferta, publicacion) =>
                {
                    oferta.Oferta_ProdOfer = new List<PublicacionSalida>();
                    oferta.Oferta_ProdOfer.Add(publicacion);
                    return oferta;
                },
                param: new { Oferta_UsuarioID = pId },
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

                OfertaSalida ofertaSalida = await dbConnection.QueryFirstOrDefaultAsync<OfertaSalida>(_ofertaQuerys.procesoAlmCrear, parameters, commandType: CommandType.StoredProcedure);

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
            

            string actualizarConsultaQuery = _ofertaQuerys.procesoAlmEdit;
            int filasAfectadas = await dbConnection.ExecuteAsync(
                actualizarConsultaQuery,
                parametros,
                commandType: CommandType.StoredProcedure
            );
           
            return filasAfectadas > 0;
        }
    }
}






