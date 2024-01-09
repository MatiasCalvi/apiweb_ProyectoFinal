using Configuracion;
using Dapper;
using Datos.Interfaces.IDaos;
using Datos.Querys;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDUsuarioAcceso : IDaoBDUsuarioAcceso
    {
        private readonly string connectionString;

        public DaoBDUsuarioAcceso(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<string> ObtenerRefreshToken(int pUsuarioId)
        { 
            using (IDbConnection dbConnection = CreateConnection())
            {
                dbConnection.Open();

                var refreshToken = await dbConnection.QueryFirstOrDefaultAsync<string>(
                    AccesoQuerys.existeTokenQuery, 
                    new { Usuario_ID = pUsuarioId }
                );

                return refreshToken;
            }
        }

        public async Task GuardarRefreshToken(int pUsuarioId, string pRefreshToken)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var existingToken = await dbConnection.QueryFirstOrDefaultAsync<string>(
                AccesoQuerys.existeTokenQuery, 
                new { Usuario_ID = pUsuarioId });

            if (existingToken != null)
            {
                await dbConnection.ExecuteAsync(
                    AccesoQuerys.actualizarTokenQuery,
                    new { Usuario_ID = pUsuarioId, 
                    RefreshToken = pRefreshToken 
                });
            }
            else
            {
                await dbConnection.ExecuteAsync(
                    AccesoQuerys.crearTokenQuery, 
                    new { Usuario_ID = pUsuarioId, 
                    RefreshToken = pRefreshToken 
                });
            }      
        }

        public async Task EliminarRefreshToken(int pUsuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            await dbConnection.ExecuteAsync(
                AccesoQuerys.eliminarTokenQuery, 
                new { Usuario_ID = pUsuarioId 
            });
        }
    }
}
