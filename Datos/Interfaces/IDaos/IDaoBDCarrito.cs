using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDCarrito
    {
        Task<List<CarritoSalida>> ObtenerCarrito(int pUsuarioID);
        Task<bool> Agregar(int pUsuarioID, int pPublicacionID);
        Task<bool> Duplicado(int pUsuarioID, int pPublicacionID);
    }
}
