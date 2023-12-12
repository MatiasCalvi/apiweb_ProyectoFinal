using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;
using Configuracion;
using Datos.Modelos.DTO;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Modelos;
using System.Data.Common;

namespace Datos
{
    public class DaoBDUsuarios : IDaoBDUsuarios
    {
        private readonly string connectionString;
        private const string obtenerUsuarioIDQuery = "SELECT * FROM usuarios WHERE Usuario_ID = @Usuario_ID";
        private const string obtenerUsuarioEmailQuery = "SELECT * FROM usuarios WHERE Usuario_Email = @Usuario_Email";
        private const string esAdministradorQuery = "SELECT COUNT(*) FROM email_admins WHERE EA_Email = @EA_Email";
        private const string crearUsuarioQuery = "INSERT INTO usuarios(Usuario_Nombre, Usuario_Apellido, Usuario_Email, Usuario_Contra, Usuario_FCreacion, Usuario_Role) VALUES(@Usuario_Nombre, @Usuario_Apellido, @Usuario_Email, @Usuario_Contra, @Usuario_FCreacion, @Usuario_Role); SELECT* FROM usuarios WHERE Usuario_ID = LAST_INSERT_ID()";

        public DaoBDUsuarios(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<UsuarioSalida?> ObtenerUsuarioPorID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<UsuarioSalida>(obtenerUsuarioIDQuery, new { Usuario_ID = pId })).FirstOrDefault();
        }

        public async Task<UsuarioModif?> ObtenerUsuarioPorIDU(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<UsuarioModif>(obtenerUsuarioIDQuery, new { Usuario_ID = pId })).FirstOrDefault();
        }

        public async Task<UsuarioSalida> ObtenerUsuarioPorEmail(string pEmail)
        {   
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return await dbConnection.QueryFirstOrDefaultAsync<UsuarioSalida>(obtenerUsuarioEmailQuery, new { Usuario_Email = pEmail });   
        }

        public async Task<UsuarioModif> ObtenerUsuarioPorEmailU(string pEmail)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return await dbConnection.QueryFirstOrDefaultAsync<UsuarioModif>(obtenerUsuarioEmailQuery, new { Usuario_Email = pEmail }); 
        }

        public async Task<bool> EsAdministrador(string pEmail)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int conteo = await dbConnection.ExecuteScalarAsync<int>(esAdministradorQuery, new { EA_Email = pEmail });

            return conteo > 0;
        }

        public async Task<UsuarioSalidaC> CrearNuevoUsuario(UsuarioCreacion pUsuario)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return await dbConnection.QuerySingleAsync<UsuarioSalidaC>(crearUsuarioQuery, pUsuario);
        }

        public async Task<bool> ActualizarUsuario(int pId, UsuarioModif pUsuarioModif)
        {  
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            List<string> actualizarCampos = new();
            DynamicParameters parametros = new();

            if (pUsuarioModif.Usuario_Nombre != null)
            {
                actualizarCampos.Add("Usuario_Nombre = @Usuario_Nombre");
                parametros.Add("Usuario_Nombre", pUsuarioModif.Usuario_Nombre);
            }

            if (pUsuarioModif.Usuario_Apellido != null)
            {
                actualizarCampos.Add("Usuario_Apellido = @Usuario_Apellido");
                parametros.Add("Usuario_Apellido", pUsuarioModif.Usuario_Apellido);
            }

            if (pUsuarioModif.Usuario_Email != null)
            {
                actualizarCampos.Add("Usuario_Email = @Usuario_Email");
                parametros.Add("Usuario_Email", pUsuarioModif.Usuario_Email);
            }

            if (pUsuarioModif.Usuario_Contra != null)
            {
                actualizarCampos.Add("Usuario_Contra = @Usuario_Contra");
                parametros.Add("Usuario_Contra", pUsuarioModif.Usuario_Contra);
            }

            if (actualizarCampos.Count == 0) return false;

            parametros.Add("Usuario_ID", pId);
            actualizarCampos.Add("Usuario_FModif = @Usuario_FModif");
            parametros.Add("Usuario_FModif", pUsuarioModif.Usuario_FModif);

            string actualizarConsultaQuery = $"UPDATE usuarios SET {string.Join(", ", actualizarCampos)} WHERE Usuario_ID = @Usuario_ID";

            int filasAfectadas = await dbConnection.ExecuteAsync(actualizarConsultaQuery, parametros);

            return filasAfectadas > 0;   
        }
    }
}
