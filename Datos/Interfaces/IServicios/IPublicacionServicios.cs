using Datos.Modelos.DTO;
using Datos.Modelos;

namespace Datos.Interfaces.IServicios
{
    public interface IPublicacionServicios
    {
        Task<List<PublicacionSalida>> Buscar(string pPalabraClave);
        Task<List<PublicacionSalida>> ObtenerPublicaciones();
        Task<PublicacionSalida> ObtenerPublicacionPorID(int pId);
        Task<PublicacionSalidaM> ObtenerPublicacionPorIDM(int pId);
        Task<PublicacionSalidaE> ObtenerPublicacionPorIDE(int pId);
        Task<PublicacionSalida> ObtenerStock(int pId);
        Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID);
        Task<PublicacionSalidaC> CrearPublicacion(PublicacionCreacion pPublicacion);
        Task<bool> EditarPublicacion(int pId, PublicacionModif pPublicacionModif);
        Task<bool> PausarPublicacion(int pId, int pUsuarioID);
        Task<bool> CancelarPublicacion(int pId, int pUsuarioID);
        Task<bool> ActivarPublicacion(int pId, int pNuevoStock);
        Task<bool> VerificarPublicPausada(int pId);
        Task<bool> VerificarPublicCancelada(int pId);
        Task<bool> VerificarPublicActivada(int pId);
        Task<bool> PausarPublicacionAdmin(int pId);
        Task<bool> CancelarPublicacionAdmin(int pId);
        Task<bool> ActivarPublicacionAdmin(int pId);
    }
}
