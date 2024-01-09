using Configuracion;
using Dapper;
using Datos.Interfaces.IDaos;
using Datos.Modelos.DTO;
using Datos.Querys;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDHistorias : IDaoBDHistorias
    {
        private readonly string connectionString;

        public DaoBDHistorias(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
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

           return (await dbConnection.QueryAsync<HistoriaCompraSalida>(
               HistoriaQuerys.obtenerHistorialQuery, 
               new { HC_UsuarioID = pUsuarioID }
           )).ToList();
        }
    }
}
