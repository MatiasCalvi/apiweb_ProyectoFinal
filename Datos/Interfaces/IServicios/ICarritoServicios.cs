using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface ICarritoServicios
    {
        Task<List<CarritoSalida>> ObtenerCarrito(int pUsuarioID);
        Task<bool> Agregar(int pUsuarioID, int pId);
        Task<bool> Duplicado(int pUsuarioID, int pId);
    }
}
