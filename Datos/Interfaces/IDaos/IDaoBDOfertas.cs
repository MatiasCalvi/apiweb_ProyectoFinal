using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDOfertas
    {
        Task<OfertaSalida?> ObtenerOfertaPorID(int pId);
    }
}
