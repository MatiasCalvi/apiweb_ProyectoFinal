using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface ICarritoServicios
    {
        Task<List<CarritoSalida>> ObtenerCarrito(int pUsuarioID);
        Task<bool> Agregar(int pUsuarioID, Carrito pCarrito);
        Task<bool> AgregarAlHistorial(int pId, int pUnidadesProducto);
        Task<bool> ReducirStock(int publicacionID, int unidades);
        Task<bool> Eliminar(int pUsuarioID, int pId);
        Task<bool> EliminarTodo(int pUsuarioID);
        Task<bool> Duplicado(int pUsuarioID, int pId);
    }
}
