using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class CarritoQuerys : ICarritoQuerys
    {
        public string obtenerCarritoQuery { get; set; } = "SELECT c.*, p.* FROM carrito c INNER JOIN publicaciones p ON c.Carrito_PID = p.Public_ID WHERE c.Carrito_UsuarioID = @Carrito_UsuarioID";
        public string agregarProducto { get; set; } = "INSERT INTO carrito (Carrito_UsuarioID,Carrito_PID) VALUES (@Carrito_UsuarioID,@Carrito_PID);";
        public string verificarDuplicado { get; set; } = "SELECT COUNT(*) FROM carrito WHERE Carrito_UsuarioID = @Carrito_UsuarioID AND Carrito_PID = @Carrito_PID";
        public string comprarQuery { get; set; } = "";
    }
}
