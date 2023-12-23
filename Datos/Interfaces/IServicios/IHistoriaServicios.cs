using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface IHistoriaServicios
    {
        Task<List<HistoriaCompraSalida>> ObtenerHistorial(int pUsuarioID);
    }
}
