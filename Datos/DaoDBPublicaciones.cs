using System.Data;
using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IQuerys;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Datos.Querys;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Datos
{
    public class DaoBDPublicaciones : IDaoBDPublicaciones
    {
        private readonly string connectionString;
        private IPublicacionQuerys _publicidadQuerys;

        public DaoBDPublicaciones(IOptions<BDConfiguration> dbConfig, IPublicacionQuerys publicidadQuerys)
        {
            connectionString = dbConfig.Value.ConnectionString;
            _publicidadQuerys = publicidadQuerys;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<PublicacionSalida>> ObtenerPublicaciones()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<PublicacionSalida>(_publicidadQuerys.obtenerPublicacionesQuery)).ToList();
        }

        public async Task<PublicacionSalida?> ObtenerPublicacionPorID(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<PublicacionSalida>(_publicidadQuerys.obtenerPublicacionIDQuery, new { Public_ID = pId })).FirstOrDefault();
        }

        public async Task<PublicacionSalidaM?> ObtenerPublicacionPorIDM(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<PublicacionSalidaM>(_publicidadQuerys.obtenerPublicacionIDQuery, new { Public_ID = pId })).FirstOrDefault();
        }

        public async Task<PublicacionSalida?> ObtenerPublicacionPorIDE(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<PublicacionSalida>(_publicidadQuerys.obtenerPublicacionIDQuery, new { Public_ID = pId })).FirstOrDefault();
        }

        public async Task<PublicacionSalida> ObtenerStock(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<PublicacionSalida>(_publicidadQuerys.obtenerStockPublicacionQuery, new { Public_ID = pId })).FirstOrDefault();
        }

        public async Task<List<PublicacionSalida>> Buscar(string pPalabraClave)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            string procedimientoBuscar = _publicidadQuerys.proceBuscar;

            var productos = await dbConnection.QueryAsync<PublicacionSalida>(procedimientoBuscar, new { Texto = pPalabraClave }, commandType: CommandType.StoredProcedure);

            return productos.ToList();
        }

        public async Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryAsync<PublicacionSalida>(_publicidadQuerys.obtenerPublicacionesDeUnUsuarioQuery, new { Public_UsuarioID = pUsuarioID })).ToList();
        }

        public async Task<PublicacionSalidaC> CrearPublicacion(PublicacionCreacion pPublicacion)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                return await dbConnection.QuerySingleAsync<PublicacionSalidaC>(_publicidadQuerys.crearPublicacionQuery, pPublicacion);
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al crear una nueva publicación: {ex.Message}");
            }
        }
        public async Task<bool> PausarPublicacion(int pId, int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(_publicidadQuerys.PausarPublicQuery, new {Public_ID = pId, Public_UsuarioID = pUsuarioID });
            
            return filasAfectadas > 0;
        }

        public async Task<bool> CancelarPublicacion(int pId,int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(_publicidadQuerys.CancelarPublicQuery, new { Public_ID = pId, Public_UsuarioID = pUsuarioID });

            return filasAfectadas > 0;
        }

        public async Task<bool> ActivarPublicacion(int pId, int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(_publicidadQuerys.ActivarPublicQuery, new { Public_ID = pId, Public_UsuarioID = pUsuarioID });

            return filasAfectadas > 0;
        }

        public async Task<bool> PausarPublicacionAdmin(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(_publicidadQuerys.PausarPublicAdminQuery, new { Public_ID = pId });

            return filasAfectadas > 0;
        }

        public async Task<bool> CancelarPublicacionAdmin(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(_publicidadQuerys.CancelarPublicAdminQuery, new { Public_ID = pId});

            return filasAfectadas > 0;
        }

        public async Task<bool> ActivarPublicacionAdmin(int pId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(_publicidadQuerys.ActivarPublicAdminQuery, new { Public_ID = pId });

            return filasAfectadas > 0;
        }

        public async Task<bool> VerificarPublicPausada(int pPublicId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryFirstOrDefaultAsync<int>(_publicidadQuerys.VerificarPublicPausadaQuery, new { Public_ID = pPublicId })) == 1;
        }

        public async Task<bool> VerificarPublicCancelada(int pPublicId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryFirstOrDefaultAsync<int>(_publicidadQuerys.VerificarPublicCanceladaQuery, new { Public_ID = pPublicId })) == 1;
        }

        public async Task<bool> VerificarPublicActivada(int pPublicId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryFirstOrDefaultAsync<int>(_publicidadQuerys.VerificarPublicActivadaQuery, new { Public_ID = pPublicId })) == 1;
        }

        public async Task<bool> EditarPublicacion(int pId, PublicacionModif pPublicacionModif)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            List<string> actualizarCampos = new();
            DynamicParameters parametros = new();

            if (pPublicacionModif.Public_Nombre != null)
            {
                actualizarCampos.Add("Public_Nombre = @Public_Nombre");
                parametros.Add("Public_Nombre", pPublicacionModif.Public_Nombre);
            }

            if (pPublicacionModif.Public_Descripcion != null)
            {
                actualizarCampos.Add("Public_Descripcion = @Public_Descripcion");
                parametros.Add("Public_Descripcion", pPublicacionModif.Public_Descripcion);
            }

            if (pPublicacionModif.Public_Precio != null)
            {
                actualizarCampos.Add("Public_Precio = @Public_Precio");
                parametros.Add("Public_Precio", pPublicacionModif.Public_Precio);
            }

            if (pPublicacionModif.Public_Imagen != null)
            {
                actualizarCampos.Add("Public_Imagen = @Public_Imagen");
                parametros.Add("Public_Imagen", pPublicacionModif.Public_Imagen);
            }

            if (pPublicacionModif.Public_Stock != null)
            {
                actualizarCampos.Add("Public_Stock = @Public_Stock");
                parametros.Add("Public_Stock", pPublicacionModif.Public_Stock);
            }
            
            if (actualizarCampos.Count == 0) return false;

            parametros.Add("Public_ID", pId);
            actualizarCampos.Add("Public_FModif = @Public_FModif");
            parametros.Add("Public_FModif", pPublicacionModif.Public_FModif);

            string actualizarConsultaQuery = $"UPDATE publicaciones SET {string.Join(", ", actualizarCampos)} WHERE Public_ID = @Public_ID";

            int filasAfectadas = await dbConnection.ExecuteAsync(actualizarConsultaQuery, parametros);

            return filasAfectadas > 0;
          
        }
    }
}

