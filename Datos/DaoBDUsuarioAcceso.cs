using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDUsuarioAcceso : IDaoBDUsuarioAcceso
    {
        private readonly string connectionString;
        private const string existeTokenQuery = "SELECT RefreshTU_Token FROM refreshtokenu WHERE refreshTU_UsuaID = @Usuario_ID";
        private const string actualizarTokenQuery = "UPDATE refreshtokenu SET RefreshTU_Token = @RefreshToken WHERE refreshTU_UsuaID = @Usuario_ID";
        private const string crearTokenQuery = "INSERT INTO refreshtokenu (RefreshTU_UsuaID, RefreshTU_Token) VALUES (@Usuario_ID, @RefreshToken)";
        private const string eliminarTokenQuery = "DELETE FROM refreshtokenu WHERE RefreshTU_UsuaID = @Usuario_ID";

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
            try
            {
                using (IDbConnection dbConnection = CreateConnection())
                {
                    dbConnection.Open();

                    var refreshToken = await dbConnection.QueryFirstOrDefaultAsync<string>(existeTokenQuery, new { Usuario_ID = pUsuarioId }
                    );

                    return refreshToken;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Error getting the refresh token from the database.", ex);
            }
        }

        public async Task GuardarRefreshToken(int pUsuarioId, string pRefreshToken)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                var existingToken = await dbConnection.QueryFirstOrDefaultAsync<string>(existeTokenQuery, new { Usuario_ID = pUsuarioId });

                if (existingToken != null)
                {
                    await dbConnection.ExecuteAsync(actualizarTokenQuery, new { Usuario_ID = pUsuarioId, RefreshToken = pRefreshToken });
                }
                else
                {
                    await dbConnection.ExecuteAsync(crearTokenQuery, new { Usuario_ID = pUsuarioId, RefreshToken = pRefreshToken });
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error al almacenar el token de actualización en la base de datos.", ex);
            }
        }

        public async Task EliminarRefreshToken(int pUsuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                await dbConnection.ExecuteAsync(eliminarTokenQuery, new { Usuario_ID = pUsuarioId });
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al eliminar el token de actualización para el usuario con el ID: {pUsuarioId}.", ex);
            }
        }
    }
}
