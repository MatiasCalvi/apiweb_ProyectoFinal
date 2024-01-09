using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface IAdminServicios
    {
        Task<List<UsuarioSalida>> ObtenerTodosLosUsuarios();
        Task<List<CarritoSalida>> ObtenerCarritos();
        Task<List<PublicacionSalida>> ObtenerPublicaciones();
        Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID);
        Task<List<HistoriaCompraSalida>> ObtenerHistoriales();
        Task<List<HistoriaCompraSalida>> ObtenerHistorial(int pUsuarioID);
        Task<List<OfertaSalida>> ObtenerOfertas();
        Task<bool> EditarPublicacion(int pId, PublicacionModifA pPublicacionModif);
        Task<bool> EditarOfertaAdmin(int pId, OfertaModifA pOfertaModif);
        Task<bool> VerificarUsuarioHabilitado(int pId);
        Task<bool> VerificarUsuarioDeshabilitado(int pId);
        Task<bool> HabilitarUsuario(int pId);
        Task<bool> DeshabilitarUsuario(int pId);
        Task<bool> AsignarRolAAdmin(int pId);
        Task<bool> AsignarRolAUsuario(int pId);
        Task<bool> DesasociarPublicaciones(int pId);
    }
}
