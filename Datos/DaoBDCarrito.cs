﻿using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IQuerys;
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
        private ICarritoQuerys _carritoQuery;

        public DaoBDCarrito(IOptions<BDConfiguration> dbConfig, ICarritoQuerys carritoQuery)
        {
            connectionString = dbConfig.Value.ConnectionString;
            _carritoQuery = carritoQuery;
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
            
            List<dynamic> listaDinamica = (await dbConnection.QueryAsync<dynamic>(_carritoQuery.obtenerCarritoQuery, new { Carrito_UsuarioID = pUsuarioID })).ToList(); 
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
                                                            obj.Carrito_Estado, 
                                                            publicacion);
                
                listaCarritoSalida.Add(carrito);
            }
            return listaCarritoSalida; 
        }


        public async Task<bool> Agregar(int pUsuarioID, int pPublicacionID)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                
                int result = await dbConnection.ExecuteAsync(_carritoQuery.agregarProducto, new { Carrito_UsuarioID = pUsuarioID, Carrito_PID = pPublicacionID });
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al crear una nueva publicación: {ex.Message}");
            }
        }

        public async Task<bool> Comprar(int pUsuarioID, int pPublicacionID)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                int result = await dbConnection.ExecuteAsync(_carritoQuery.comprarQuery, new { Carrito_UsuarioID = pUsuarioID, Carrito_PID = pPublicacionID });
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error al crear una nueva publicación: {ex.Message}");
            }
        }

        

        public async Task<bool> Duplicado(int pUsuarioID, int pPublicacionID)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                int result = await dbConnection.QueryFirstOrDefaultAsync<int>(_carritoQuery.verificarDuplicado, new { Carrito_UsuarioID = pUsuarioID, Carrito_PID = pPublicacionID });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar duplicado: {ex.Message}");
                throw; 
            }
        }
    }
}