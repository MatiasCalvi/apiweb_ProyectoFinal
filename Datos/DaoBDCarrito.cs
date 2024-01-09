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
    public class DaoBDCarrito : IDaoBDCarrito
    {
        private readonly string connectionString;

        public DaoBDCarrito(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<CarritoSalida>> ObtenerCarrito(int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();
            
            List<dynamic> listaDinamica = (await dbConnection.QueryAsync<dynamic>(
                CarritoQuerys.obtenerCarritoQuery, 
                new { Carrito_UsuarioID = pUsuarioID }
            )).ToList();
            
            List<CarritoSalida> listaCarritoSalida = new List<CarritoSalida>(); 
            
            foreach (dynamic obj in listaDinamica) 
            {
                
                PublicacionSalida publicacion = new PublicacionSalida(obj.Public_ID,
                                                                        obj.Public_UsuarioID, 
                                                                        obj.Public_Nombre, 
                                                                        obj.Public_Descripcion, 
                                                                        obj.Public_Precio,
                                                                        obj.Public_Imagen, 
                                                                        obj.Public_Stock,
                                                                        obj.Public_Estado);
                
                CarritoSalida carrito = new CarritoSalida(obj.Carrito_UsuarioID, 
                                                            obj.Carrito_PID,
                                                            obj.Carrito_ProdUnidades,
                                                            publicacion);
                
                listaCarritoSalida.Add(carrito);
            }
            return listaCarritoSalida; 
        }

        public async Task<bool> Agregar(int pUsuarioID, CarritoCreacion pCarrito)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                
                int result = await dbConnection.ExecuteAsync(
                    CarritoQuerys.agregarProducto, 
                    new { Carrito_UsuarioID = pUsuarioID, 
                    Carrito_PID = pCarrito.Carrito_PID, 
                    Carrito_ProdUnidades = pCarrito.Carrito_ProdUnidades 
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al agregar un nuevo producto al carrito: {ex.Message}");
            }
        }

        public async Task<bool> AgregarAlHistorial(Historia pHistoriaCompra)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            var result = await dbConnection.ExecuteAsync(
                CarritoQuerys.agregarAlHistorialQuery, 
               pHistoriaCompra 
            );

            return result > 0;
        }

        public async Task<bool> Eliminar(int pUsuarioID, int pPublicacionID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int result = await dbConnection.ExecuteAsync(
                CarritoQuerys.eliminarQuery, 
                new { Carrito_UsuarioID = pUsuarioID, 
                Carrito_PID = pPublicacionID 
            });

            return result > 0;
        }

        public async Task<bool> EliminarTodo(int pUsuarioID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int result = await dbConnection.ExecuteAsync(
                CarritoQuerys.eliminarTodoQuery, 
                new { Carrito_UsuarioID = pUsuarioID
            });

            return result > 0;
        }

        public async Task<bool> Duplicado(int pUsuarioID, int pPublicacionID)
        {
            using IDbConnection dbConnection = CreateConnection();
            dbConnection.Open();

            int result = await dbConnection.QueryFirstOrDefaultAsync<int>(
                CarritoQuerys.verificarDuplicado, 
                new { Carrito_UsuarioID = pUsuarioID, 
                Carrito_PID = pPublicacionID 
            });

            return result > 0;
        }
    }
}