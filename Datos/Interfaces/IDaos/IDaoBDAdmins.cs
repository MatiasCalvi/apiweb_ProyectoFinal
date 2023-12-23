using Datos.Modelos;
using Datos.Modelos.DTO;
using System.Threading.Tasks;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDAdmins 
    {
        Task<List<PublicacionSalida>> ObtenerPublicaciones();
        Task<List<UsuarioSalida>> ObtenerTodosLosUsuarios();
        Task<List<CarritoSalida>> ObtenerCarritos();
        Task<List<HistoriaCompraSalida>> ObtenerHistoriales();
        Task<bool> VerificarUsuarioDeshabilitado(int usuarioId);
        Task<bool> VerificarUsuarioHabilitado(int usuarioId);
        Task<bool> HabilitarUsuario(int pUsuarioId);
        Task<bool> DeshabilitarUsuario(int pUsuarioId);
        Task<bool> AsignarRolAAdmin(int pUsuarioId);
        Task<bool> AsignarRolAUsuario(int usuarioId);
        Task<bool> EditarPublicacion(int pId, PublicacionModifA pPublicacionModif);
    }
}
