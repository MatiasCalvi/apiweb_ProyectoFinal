﻿using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class CarritoQuerys : ICarritoQuerys
    {
        public string obtenerCarritoQuery { get; set; } = "SELECT c.Carrito_UsuarioID, p.Public_ID as Carrito_PID, c.Carrito_ProdUnidades, p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM carrito c JOIN publicaciones p ON c.Carrito_PID = p.Public_ID JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE c.Carrito_UsuarioID = @Carrito_UsuarioID";
        public string agregarProducto { get; set; } = "INSERT INTO carrito (Carrito_UsuarioID,Carrito_PID,Carrito_ProdUnidades) VALUES (@Carrito_UsuarioID,@Carrito_PID,@Carrito_ProdUnidades)";
        public string verificarDuplicado { get; set; } = "SELECT COUNT(*) FROM carrito WHERE Carrito_UsuarioID = @Carrito_UsuarioID AND Carrito_PID = @Carrito_PID";
        public string agregarAlHistorialQuery { get; set; } = "INSERT INTO historial_compras(HC_UsuarioID,HC_PID,HC_Unidades,HC_PrecioFinal,HC_FechaCompra) VALUES(@HC_UsuarioID, @HC_PID,@HC_Unidades, @HC_PrecioFinal,@HC_FechaCompra)";
        public string eliminarQuery { get; set; } = "DELETE FROM carrito WHERE Carrito_UsuarioID = @Carrito_UsuarioID AND Carrito_PID = @Carrito_PID";
        public string eliminarTodoQuery { get; set; } = "DELETE FROM carrito WHERE Carrito_UsuarioID = @Carrito_UsuarioID";
    }
}
