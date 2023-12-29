using Datos.Interfaces.IModelos;

namespace Datos.Modelos.DTO
{
    public class CarritoCreacion
    {
        public int Carrito_PID { get; set; }
        public int Carrito_ProdUnidades { get; set; }
    }
    public class CarritoElim
    {
        public int Carrito_PID { get; set; }
    }
}
