using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface IUsuarioServicios
    {
        Task<UsuarioSalida> ObtenerUsuarioPorID(int pId);
        Task<UsuarioSalida> ObtenerUsuarioPorEmail(string pEmail);
        Task<UsuarioSalidaC> CrearNuevoUsuario(UsuarioCreacion pUsuario);
        Task<bool> ActualizarUsuario(int pId, UsuarioModif pUsuarioModif);
    }
}
