using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IQuerys;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDUsuarioAcceso : IDaoBDUsuarioAcceso
    {
        private readonly string connectionString;
        private IAccesoQuerys _accesoQuerys;

        public DaoBDUsuarioAcceso(IOptions<BDConfiguration> dbConfig, IAccesoQuerys accesoQuerys)
        {
            connectionString = dbConfig.Value.ConnectionString;
            _accesoQuerys = accesoQuerys;
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

                    var refreshToken = await dbConnection.QueryFirstOrDefaultAsync<string>(
                        _accesoQuerys.existeTokenQuery, 
                        new { Usuario_ID = pUsuarioId }
                    );

                    return refreshToken;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Error al obtener el token de actualización de la base de datos.", ex);
            }
        }

        public async Task GuardarRefreshToken(int pUsuarioId, string pRefreshToken)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                var existingToken = await dbConnection.QueryFirstOrDefaultAsync<string>(
                    _accesoQuerys.existeTokenQuery, 
                    new { Usuario_ID = pUsuarioId });

                if (existingToken != null)
                {
                    await dbConnection.ExecuteAsync(
                        _accesoQuerys.actualizarTokenQuery,
                        new { Usuario_ID = pUsuarioId, 
                        RefreshToken = pRefreshToken 
                    });
                }
                else
                {
                    await dbConnection.ExecuteAsync(
                        _accesoQuerys.crearTokenQuery, 
                        new { Usuario_ID = pUsuarioId, 
                        RefreshToken = pRefreshToken 
                    });
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
                await dbConnection.ExecuteAsync(
                    _accesoQuerys.eliminarTokenQuery, 
                    new { Usuario_ID = pUsuarioId 
                });
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al eliminar el token de actualización para el usuario con el ID: {pUsuarioId}.", ex);
            }
        }
    }
}
