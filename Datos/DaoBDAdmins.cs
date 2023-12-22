﻿using Configuracion;
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
    public class DaoBDAdmins : IDaoBDAdmins
    {
        private readonly string connectionString;
        private IAdminQuerys _adminQuery;

        public DaoBDAdmins(IOptions<BDConfiguration> dbConfig, IAdminQuerys adminQuerys)
        {
            connectionString = dbConfig.Value.ConnectionString;
            _adminQuery = adminQuerys;
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
                return (await dbConnection.QueryAsync<UsuarioSalida>(_adminQuery.obtenerUsuariosQuery)).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Error al obtener todos los usuarios.", ex);
            }
        }

        public async Task<List<PublicacionSalida>> ObtenerPublicaciones()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<PublicacionSalida>(_adminQuery.obtenerPublicacionesQuery)).ToList();
        }

        public async Task<List<CarritoSalida>> ObtenerCarritos()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            List<dynamic> listaDinamica = (await dbConnection.QueryAsync<dynamic>(_adminQuery.obtenerCarritosQuery)).ToList(); 
            List<CarritoSalida> listaCarritoSalida = new List<CarritoSalida>();
            
            foreach (dynamic obj in listaDinamica) 
            {
                PublicacionSalida publicacion = new PublicacionSalida(obj.Public_UsuarioID, 
                                                                        obj.Carrito_PID, 
                                                                        obj.Public_Nombre,
                                                                        obj.Public_Descripcion, 
                                                                        obj.Public_Precio, 
                                                                        obj.Public_Imagen, 
                                                                        obj.Public_Stock);

                CarritoSalida carrito = new CarritoSalida(obj.Carrito_UsuarioID,
                                            obj.Carrito_PID,
                                            obj.Carrito_ProdUnidades,
                                            publicacion);

                listaCarritoSalida.Add(carrito);
            }
            return listaCarritoSalida; 
        }

        public async Task<bool> VerificarUsuarioHabilitado(int usuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                return (await dbConnection.QueryFirstOrDefaultAsync<int>(_adminQuery.verificarUsuarioHabilitadoQuery, new { UsuarioId = usuarioId })) == 1;
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

                return (await dbConnection.QueryFirstOrDefaultAsync<int>(_adminQuery.verificarUsuarioDeshabilitadoQuery, new { UsuarioId = usuarioId })) == 1;
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
                int filasAfectadas = await dbConnection.ExecuteAsync(_adminQuery.habilitarUsuarioQuery, new { Usuario_ID = pUsuarioId });

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
                int filasAfectadas = await dbConnection.ExecuteAsync(_adminQuery.desactivarUsuarioQuery, new { Usuario_ID = pUsuarioId });

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al deshabilitar usuario con ID {pUsuarioId}.", ex);
            }
        }

        public async Task<bool> AsignarRolAAdmin(int pUsuarioId)
        {
           
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                int filasAfectadas = await dbConnection.ExecuteAsync(_adminQuery.asignarRolAAdminQuery, new { Usuario_ID = pUsuarioId });

                return filasAfectadas > 0;
            
        }

        public async Task<bool> AsignarRolAUsuario(int pUsuarioId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                int filasAfectadas = await dbConnection.ExecuteAsync(_adminQuery.asignarRolAUsuarioQuery, new { Usuario_ID = pUsuarioId });

                return filasAfectadas > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al asignar el rol al usuario con ID {pUsuarioId}.", ex);
            }
        }

        public async Task<bool> EditarPublicacion(int pId, PublicacionModifA pPublicacionModif)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            List<string> actualizarCampos = new();
            DynamicParameters parametros = new();

            if (pPublicacionModif.Public_UsuarioID != null)
            {
                actualizarCampos.Add("Public_UsuarioID = @Public_UsuarioID");
                parametros.Add("Public_UsuarioID", pPublicacionModif.Public_UsuarioID);
            }

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
