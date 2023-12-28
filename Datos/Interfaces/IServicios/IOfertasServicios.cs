using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface IOfertasServicios
    {
        Task<List<OfertaSalida>> ObtenerOfertas();
        Task<OfertaSalida> ObtenerOfertaPorID(int pId);
        Task<List<OfertaSalida>> ObtenerOfertasPorUsuarioID(int pId);
        Task<bool> VerificarAutoria(int pUsuarioID, List<int?>? pOfertasID);
        Task<OfertaSalidaC> CrearOferta(OfertaCreacion oferta);
        Task<bool> EditarOferta(int pId, OfertaModif pOfertaModif);
        Task<bool> EliminarOferta(int pOfertaID);
        Task<bool> EliminarOfertas(int pUsuarioID);
    }
}
