using Datos.Interfaces.IModelos;

namespace Datos.Modelos.DTO
{
    public class CarritoSalida : ICarrito
    {
        public int Carrito_UsuarioID { get; set; }
        public int Carrito_PID { get; set; }
        public int Carrito_Estado { get; set; }
        public PublicacionSalida Publicacion { get; set; }

        public CarritoSalida() { }

        public CarritoSalida(int carrito_UsuarioID, int carrito_PID, int carrito_Estado, PublicacionSalida publicacion)
        {
            Carrito_UsuarioID = carrito_UsuarioID;
            Carrito_PID = carrito_PID;
            Carrito_Estado = carrito_Estado;
            Publicacion = publicacion; 
        }
    }
}
