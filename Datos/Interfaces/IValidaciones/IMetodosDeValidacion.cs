using Datos.Modelos.DTO;

namespace Datos.Interfaces.IValidaciones
{
    public interface IMetodosDeValidacion
    {
        Task<string> HashContra(string pPassword);
        Task<bool> VerificarContra(string pUserInput, string pHashedPassword);
        Task<UsuarioSalida> VerificarUsuario(string pEmail, string pPassword);
        Task<int> ObtenerUsuarioIDToken();
        Task<string> ObtenerUsuarioRoleToken();
        Task<string> ObtenerRefreshToken(int pUsuario);
        string GenerarTokenAcceso(UsuarioSalida usuario);
        Task<string> GenerarYGuardarRefreshToken(int userId, string userRole);
        Task EliminarRefreshToken(int userId);
        void EliminarCookie(string pNombreCookie);
        void ActualizarCookie(string refreshToken, string cookieValue);
    }
}
