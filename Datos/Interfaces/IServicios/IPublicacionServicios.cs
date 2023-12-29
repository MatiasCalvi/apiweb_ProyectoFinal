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
        Task<PublicacionSalida> ObtenerStock(int pId);
        Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID);
        Task<PublicacionSalidaC> CrearPublicacion(PublicacionCreacion pPublicacion);
        Task<bool> EditarPublicacion(int pId, PublicacionModif pPublicacionModif);
        Task<bool> CambiarEstadoPublicacion(int pId, int pEstadoID);
        Task<bool> VerificarPublicEstado(int pPublicID, int pEstado);
        Task<bool> ActivarPublicacion(int pId, int pNuevoStock);
        Task<bool> EliminarPublicacion(int pPublicacionID);
        Task<bool> EliminarPublicaciones(int pUsuarioID);
    }
}
