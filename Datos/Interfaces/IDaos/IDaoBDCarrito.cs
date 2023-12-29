using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDCarrito
    {
        Task<List<CarritoSalida>> ObtenerCarrito(int pUsuarioID);
        Task<bool> Agregar(int pUsuarioID, CarritoCreacion pCarrito);
        Task<bool> AgregarAlHistorial(Historia pHistoriaCompra);
        Task<bool> Eliminar(int pUsuarioID, int pPublicacionID);
        Task<bool> EliminarTodo(int pUsuarioID);
        Task<bool> Duplicado(int pUsuarioID, int pPublicacionID);
    }
}
