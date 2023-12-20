using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;

namespace Datos.Servicios
{
    public class AdminServicios : IAdminServicios
    {
        private IDaoBDAdmins _daoBDAdmins;

        public AdminServicios(IDaoBDAdmins daoBDAdmins)
        {
            _daoBDAdmins = daoBDAdmins;
        }

        public async Task<List<UsuarioSalida>> ObtenerTodosLosUsuarios()
        {
            return await _daoBDAdmins.ObtenerTodosLosUsuarios();
        }

        public async Task<List<CarritoSalida>> ObtenerCarritos()
        {
            return await _daoBDAdmins.ObtenerCarritos();
        }

        public async Task<bool> VerificarUsuarioHabilitado(int pId)
        {
            bool resultado = await _daoBDAdmins.VerificarUsuarioHabilitado(pId);

            return resultado;
        }

        public async Task<bool> VerificarUsuarioDeshabilitado(int pId)
        {
            bool resultado = await _daoBDAdmins.VerificarUsuarioDeshabilitado(pId);
            
            return resultado;
        }

        public async Task<bool> HabilitarUsuario(int pId)
        {
            bool resultado = await _daoBDAdmins.HabilitarUsuario(pId);

            if (!resultado)
            {
                throw new DeletionFailedException($"No se pudo habilitar el usuario con ID {pId}.");
            }

            return resultado;
        }

        public async Task<bool> DeshabilitarUsuario(int pId)
        {
            bool resultado = await _daoBDAdmins.DeshabilitarUsuario(pId);

            if (!resultado)
            {
                throw new DeletionFailedException($"No se pudo deshabilitar la usuario con ID {pId}.");
            }

            return resultado;       
        }

        public async Task<bool> AsignarRolAAdmin(int pId)
        {
            bool resultado = await _daoBDAdmins.AsignarRolAAdmin(pId);

            if (!resultado)
            {
                throw new UpdateFailedException($"No se pudo cambiar el rol del usuario con ID {pId} a admin.");
            }

            return resultado;
        }

        public async Task<bool> AsignarRolAUsuario(int pId)
        {
            bool resultado = await _daoBDAdmins.AsignarRolAUsuario(pId);

            if (!resultado)
            {
                throw new UpdateFailedException($"No se pudo asignar el rol 'usuario' al usuario con ID {pId}.");
            }

            return resultado;
        }

    }
}
