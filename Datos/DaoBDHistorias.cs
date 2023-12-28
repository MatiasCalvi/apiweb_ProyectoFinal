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
    public class DaoBDHistorias : IDaoBDHistorias
    {
        private readonly string connectionString;
        private IHistoriaQuerys _historiasQuery;

        public DaoBDHistorias(IOptions<BDConfiguration> dbConfig, IHistoriaQuerys historiasQuery)
        {
            connectionString = dbConfig.Value.ConnectionString;
            _historiasQuery = historiasQuery;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<HistoriaCompraSalida>> ObtenerHistorial(int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

           return (await dbConnection.QueryAsync<HistoriaCompraSalida>(_historiasQuery.obtenerHistorialQuery, 
                                                                        new { HC_UsuarioID = pUsuarioID })).ToList();
        }
    }
}
