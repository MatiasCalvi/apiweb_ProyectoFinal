using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDOfertas
    {
        Task<List<OfertaSalida>> ObtenerOfertas();
        Task<OfertaSalida?> ObtenerOfertaPorID(int pId);
        Task<List<OfertaSalida>> ObtenerOfertasPorUsuarioID(int pId);
        Task<bool> VerificarAutoria(int pUsuarioId, int pOfertaId);
        Task<OfertaSalida> CrearOferta(OfertaCreacion pOferta);
        Task<bool> EditarOferta(int pId, OfertaModif pOfertaModif);
        Task<bool> VerificarOfertaEstado(int pOfertaID, int pEstadoID);
        Task<bool> CambiarEstadoOferta(int pOfertaID, int pEstadoID);
        Task<bool> DesasociarPublicaciones(int pOfertaID);
        Task<bool> EliminarOferta(int? pOfertaID, int? pUsuarioID);
    }
}
