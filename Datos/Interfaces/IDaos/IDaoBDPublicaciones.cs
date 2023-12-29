using Datos.Interfaces.IModelos;
using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDPublicaciones
    {
        Task<List<PublicacionSalida>> ObtenerPublicaciones();
        Task<PublicacionSalida?> ObtenerPublicacionPorID(int pId);
        Task<PublicacionSalidaM?> ObtenerPublicacionPorIDM(int pId);
        Task<PublicacionSalida?> ObtenerPublicacionPorIDE(int pId);
        Task<PublicacionSalida?> ObtenerStock(int pId);
        Task<List<PublicacionSalida>> Buscar(string pPalabraClave);
        Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID);
        Task<PublicacionSalidaC> CrearPublicacion(PublicacionCreacion pPublicacion);
        Task<bool> EditarPublicacion(int pId, PublicacionModif pPublicacionModif);
        Task<bool> CambiarEstadoPublicacion(int pPublicID, int pEstadoID);
        Task<bool> VerificarPublicEstado(int pPublicID, int pEstadoID);
        Task<bool> EliminarPublicacion(int? pPublicacionID, int? pUsuarioID);
    }
}
