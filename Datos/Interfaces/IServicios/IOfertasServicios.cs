using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface IOfertasServicios
    {
        Task<OfertaSalida> ObtenerOfertaPorID(int pId);
    }
}
