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
        Task<PublicacionSalida> ObtenerStock(int pId);
        Task<List<PublicacionSalida>> Buscar(string pPalabraClave);
        Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID);
        Task<PublicacionSalidaC> CrearPublicacion(PublicacionCreacion pPublicacion);
        Task<bool> EditarPublicacion(int pId, PublicacionModif pPublicacionModif);
        Task<bool> PausarPublicacion(int pId, int pUsuarioID);
        Task<bool> CancelarPublicacion(int pId,int pUsuarioID);
        Task<bool> ActivarPublicacion(int pId, int pUsuarioID);
        Task<bool> PausarPublicacionAdmin(int pId);
        Task<bool> CancelarPublicacionAdmin(int pId);
        Task<bool> ActivarPublicacionAdmin(int pId);
        Task<bool> VerificarPublicPausada(int pPublicId);
        Task<bool> VerificarPublicCancelada(int pPublicId);
        Task<bool> VerificarPublicActivada(int pPublicId);

    }
}
