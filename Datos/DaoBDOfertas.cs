using Configuracion;
using Dapper;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IQuerys;
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

        public async Task<OfertaSalida> ObtenerOfertaPorID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.QueryAsync<OfertaSalida, PublicacionSalida, OfertaSalida>(
                _ofertaQuerys.obtenerOferta,
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
    }
}
