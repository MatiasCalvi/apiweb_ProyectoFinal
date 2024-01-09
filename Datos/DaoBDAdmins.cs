using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Datos.Querys;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDAdmins : IDaoBDAdmins
    {
        private readonly string connectionString;

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
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<UsuarioSalida>(AdminQuerys.obtenerUsuariosQuery)).ToList();
        }

        public async Task<List<PublicacionSalida>> ObtenerPublicaciones()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<PublicacionSalida>(AdminQuerys.obtenerPublicacionesQuery)).ToList();
        }

        public async Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryAsync<PublicacionSalida>(
                AdminQuerys.obtenerPublicacionesDeUnUsuarioQuery, 
                new { Public_UsuarioID = pUsuarioID 
            })).ToList();
        }

        public async Task<List<CarritoSalida>> ObtenerCarritos()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            List<dynamic> listaDinamica = (await dbConnection.QueryAsync<dynamic>(AdminQuerys.obtenerCarritosQuery)).ToList();
            
            List<CarritoSalida> listaCarritoSalida = new List<CarritoSalida>();
            
            foreach (dynamic obj in listaDinamica) 
            {
              
                PublicacionSalida publicacion = new PublicacionSalida(obj.Public_ID,
                                                    obj.Public_UsuarioID,
                                                    obj.Public_Nombre,
                                                    obj.Public_Descripcion,
                                                    Convert.ToDecimal(obj.Public_Precio),
                                                    obj.Public_Imagen,
                                                    obj.Public_Stock,
                                                    obj.Public_Estado.ToString());

                CarritoSalida carrito = new CarritoSalida(obj.Carrito_UsuarioID,
                                            obj.Carrito_PID,
                                            obj.Carrito_ProdUnidades,
                                            publicacion);

                listaCarritoSalida.Add(carrito);
            }
            return listaCarritoSalida; 
        }

        public async Task<List<HistoriaCompraSalida>> ObtenerHistoriales()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            return (await dbConnection.QueryAsync<HistoriaCompraSalida>(AdminQuerys.obtenerHistorialesQuery)).ToList();
        }

        public async Task<List<HistoriaCompraSalida>> ObtenerHistorial(int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryAsync<HistoriaCompraSalida>(
                AdminQuerys.obtenerHistorialQuery,
                new { HC_UsuarioID = pUsuarioID }
            )).ToList();
        }

        public async Task<List<OfertaSalida>> ObtenerTodasLasOfertas()
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            using var reader = await dbConnection.QueryMultipleAsync(AdminQuerys.procesoAlmObt,
                                                                        commandType: CommandType.StoredProcedure);

            var ofertas = reader.Read<OfertaSalida, PublicacionSalida, OfertaSalida>(
                (oferta, publicacion) =>
                {
                    if (oferta.Oferta_ProdOfer == null)
                    {
                        oferta.Oferta_ProdOfer = new List<PublicacionSalida>();
                    }

                    if (publicacion != null && !oferta.Oferta_ProdOfer.Any(p => p.Public_ID == publicacion.Public_ID))
                    {
                        oferta.Oferta_ProdOfer.Add(publicacion);
                    }

                    return oferta;
                },
                splitOn: "Public_ID"
            ).ToList();

            return ofertas.ToList();
        }

        public async Task<bool> VerificarUsuarioHabilitado(int usuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryFirstOrDefaultAsync<int>(AdminQuerys.verificarUsuarioHabilitadoQuery, 
                                                                new { UsuarioId = usuarioId })) == 1;
        }

        public async Task<bool> VerificarUsuarioDeshabilitado(int usuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            return (await dbConnection.QueryFirstOrDefaultAsync<int>(
                AdminQuerys.verificarUsuarioDeshabilitadoQuery, 
                new { UsuarioId = usuarioId 
            })) == 1;
        }

        public async Task<bool> HabilitarUsuario(int pUsuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            int filasAfectadas = await dbConnection.ExecuteAsync(
                AdminQuerys.habilitarUsuarioQuery, 
                new { Usuario_ID = pUsuarioId 
            });

            return filasAfectadas > 0;
        }

        public async Task<bool> DeshabilitarUsuario(int pUsuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(
                AdminQuerys.desactivarUsuarioQuery, 
                new { Usuario_ID = pUsuarioId 
            });

            return filasAfectadas > 0;
        }

        public async Task<bool> AsignarRolAAdmin(int pUsuarioId)
        {
           
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                int filasAfectadas = await dbConnection.ExecuteAsync(
                    AdminQuerys.asignarRolAAdminQuery, 
                    new { Usuario_ID = pUsuarioId 
                });

                return filasAfectadas > 0;
            
        }

        public async Task<bool> AsignarRolAUsuario(int pUsuarioId)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(
                AdminQuerys.asignarRolAUsuarioQuery, 
                new { Usuario_ID = pUsuarioId 
            });

            return filasAfectadas > 0;
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

        public async Task<bool> DesasociarPublicaciones(int pOfertaID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int filasAfectadas = await dbConnection.ExecuteAsync(
                AdminQuerys.desasociarPublicacionesAdmin,
                new { 
                    OfertaID = pOfertaID
                }
            );

            return filasAfectadas > 0;
        }

        public async Task<bool> EditarOfertaAdmin(int pId, OfertaModifA pOfertaModif)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var parametros = new DynamicParameters();

            parametros.Add("pOfertaNombre", pOfertaModif.Oferta_Nombre);
            parametros.Add("pOfertaDescuento", pOfertaModif.Oferta_Descuento);
            parametros.Add("pOfertaUsuarioID", pOfertaModif.Oferta_UsuarioID);
            parametros.Add("pOfertaFInicio", pOfertaModif.Oferta_FInicio);
            parametros.Add("pOfertaFFin", pOfertaModif.Oferta_FFin);
            parametros.Add("pProductosIds", string.Join(",", pOfertaModif.Oferta_ProdOfer?.Where(x => x.HasValue).Select(x => x.Value.ToString()) ?? Enumerable.Empty<string>()));
            parametros.Add("pOfertaID", pId);
            parametros.Add("pOfertaFModif", pOfertaModif.Oferta_FModif);

            int filasAfectadas = await dbConnection.ExecuteAsync(
                AdminQuerys.procesoAlmEditAdmin,
                parametros,
                commandType: CommandType.StoredProcedure
            );
            return filasAfectadas > 0;
        }
    }
}
