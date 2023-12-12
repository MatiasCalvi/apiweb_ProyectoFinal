using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Modelos.DTO;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDAdmins : IDaoBDAdmins
    {
        private readonly string connectionString;
        private const string obtenerUsuariosQuery = "SELECT * FROM usuarios";
        private const string verificarUsuarioDeshabilitadoQuery = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 0";
        private const string verificarUsuarioHabilitadoQuery = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 1";
        private const string habilitarUsuarioQuery = "UPDATE usuarios SET Usuario_Estado = 1 WHERE Usuario_ID = @Usuario_ID";
        private const string desactivarUsuarioQuery = "UPDATE usuarios SET Usuario_Estado = 0 WHERE Usuario_ID = @Usuario_ID";
        private const string asignarRolAAdminQuery = "UPDATE usuarios SET Usuario_Role = 'admin' WHERE Usuario_ID = @Usuario_ID";
        private const string asignarRolAUsuarioQuery = "UPDATE usuarios SET Usuario_Role = 'usuario' WHERE Usuario_ID = @Usuario_ID";

        public DaoBDAdmins(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<UsuarioSalida>> ObtenerTodosLosUsuarios()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return (await dbConnection.QueryAsync<UsuarioSalida>(obtenerUsuariosQuery)).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Error al obtener todos los usuarios.", ex);
            }
        }
        public async Task<bool> VerificarUsuarioHabilitado(int usuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                return (await dbConnection.QueryFirstOrDefaultAsync<int>(verificarUsuarioHabilitadoQuery, new { UsuarioId = usuarioId })) == 1;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al verificar el estado del usuario con ID {usuarioId}.", ex);
            }
        }

        public async Task<bool> VerificarUsuarioDeshabilitado(int usuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                return (await dbConnection.QueryFirstOrDefaultAsync<int>(verificarUsuarioDeshabilitadoQuery, new { UsuarioId = usuarioId })) == 1;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al verificar el estado del usuario con ID {usuarioId}.", ex);
            }
        }

        public async Task<bool> HabilitarUsuario(int pUsuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                int filasAfectadas = await dbConnection.ExecuteAsync(habilitarUsuarioQuery, new { Usuario_ID = pUsuarioId });

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al habilitar usuario con ID {pUsuarioId}.", ex);
            }
        }

        public async Task<bool> DeshabilitarUsuario(int pUsuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                int filasAfectadas = await dbConnection.ExecuteAsync(desactivarUsuarioQuery, new { Usuario_ID = pUsuarioId });

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al deshabilitar usuario con ID {pUsuarioId}.", ex);
            }
        }

        public async Task<bool> AsignarRolAAdmin(int pUsuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                int filasAfectadas = await dbConnection.ExecuteAsync(asignarRolAAdminQuery, new { Usuario_ID = pUsuarioId });

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al cambiar el rol a admin para el usuario con ID {pUsuarioId}.", ex);
            }
        }

        public async Task<bool> AsignarRolAUsuario(int pUsuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                int filasAfectadas = await dbConnection.ExecuteAsync(asignarRolAUsuarioQuery, new { Usuario_ID = pUsuarioId });

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al asignar el rol al usuario con ID {pUsuarioId}.", ex);
            }
        }

    }
}
