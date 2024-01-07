using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDOfertas
    {
        Task<List<OfertaSalida>> ObtenerOfertas(DateTime pFechaActual);
        Task<OfertaSalida?> ObtenerOfertaPorID(int pId);
        Task<List<OfertaSalida>> ObtenerOfertasPorUsuarioID(int pId);
        Task<bool> VerificarAutoria(int pUsuarioId, int pOfertaId);
        Task<OfertaSalida> CrearOferta(OfertaCreacion pOferta);
        Task<bool> EditarOferta(int pId, OfertaModif pOfertaModif);
        Task<bool> DesasociarPublicaciones(int pOfertaID);
        Task<bool> EliminarOferta(int? pOfertaID, int? pUsuarioID);
        Task<int> VerificarDescuento(int pPublicID);
    }
}
