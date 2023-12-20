using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Servicios
{
    public class UsuarioServicios : IUsuarioServicios
    {
        private IDaoBDUsuarios _daoBDUsuarios;
        private IMetodosDeValidacion _metodosDeValidacion;

        public UsuarioServicios(IDaoBDUsuarios daoBDUsuarios, IMetodosDeValidacion metodosDeValidacion)
        {
            _daoBDUsuarios = daoBDUsuarios;
            _metodosDeValidacion = metodosDeValidacion;
        }

        public async Task<UsuarioSalida> ObtenerUsuarioPorID(int pId)
        {
            return await _daoBDUsuarios.ObtenerUsuarioPorID(pId);
        }

        public async Task<UsuarioSalida> ObtenerUsuarioPorEmail(string pEmail)
        {

            UsuarioSalida usuario = await _daoBDUsuarios.ObtenerUsuarioPorEmail(pEmail);  
            return usuario;
        }

        public async Task<UsuarioSalidaC> CrearNuevoUsuario(UsuarioCreacion pUsuario)
        {
            string hashedPassword = await _metodosDeValidacion.HashContra(pUsuario.Usuario_Contra);
            pUsuario.Usuario_Contra = hashedPassword;
            pUsuario.Usuario_FCreacion = DateTime.Now;
            
            UsuarioSalidaC usuario = await _daoBDUsuarios.CrearNuevoUsuario(pUsuario);
            if (usuario == null)
            {
                 throw new CreationFailedException("Error al crear un nuevo usuario.");
            }

            return usuario;
        }

        public async Task<bool> ActualizarUsuario(int pId, UsuarioModif pUsuarioModif)
        {
            UsuarioModif usuarioActual = await _daoBDUsuarios.ObtenerUsuarioPorIDU(pId);

            DateTime fechaActual = DateTime.Now;

            bool contraseñaCambiada = pUsuarioModif.Usuario_Contra != null &&
                                       !await _metodosDeValidacion.VerificarContra(pUsuarioModif.Usuario_Contra, usuarioActual.Usuario_Contra);

            bool nombreCambiado = pUsuarioModif.Usuario_Nombre != null && pUsuarioModif.Usuario_Nombre != usuarioActual.Usuario_Nombre;
            bool apellidoCambiado = pUsuarioModif.Usuario_Apellido != null && pUsuarioModif.Usuario_Apellido != usuarioActual.Usuario_Apellido;
            bool emailCambiado = pUsuarioModif.Usuario_Email != null && pUsuarioModif.Usuario_Email != usuarioActual.Usuario_Email;

            if (contraseñaCambiada || nombreCambiado || apellidoCambiado || emailCambiado)
            {
                string hashedContra = contraseñaCambiada ? await _metodosDeValidacion.HashContra(pUsuarioModif.Usuario_Contra) : usuarioActual.Usuario_Contra;

                usuarioActual.Usuario_Contra = hashedContra;
                usuarioActual.Usuario_Nombre = nombreCambiado ? pUsuarioModif.Usuario_Nombre : usuarioActual.Usuario_Nombre;
                usuarioActual.Usuario_Apellido = apellidoCambiado ? pUsuarioModif.Usuario_Apellido : usuarioActual.Usuario_Apellido;
                usuarioActual.Usuario_Email = emailCambiado ? pUsuarioModif.Usuario_Email : usuarioActual.Usuario_Email;
                usuarioActual.Usuario_FModif = fechaActual;

                bool resultado = await _daoBDUsuarios.ActualizarUsuario(pId, usuarioActual);

                return resultado;
               
            }
            
            return false;
        }
    }
}

