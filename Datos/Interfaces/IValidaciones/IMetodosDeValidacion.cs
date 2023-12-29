using Datos.Modelos.DTO;

namespace Datos.Interfaces.IValidaciones
{
    public interface IMetodosDeValidacion
    {
        Task<string> HashContra(string pPassword);
        Task<bool> VerificarContra(string pUserInput, string pHashedPassword);
        Task<bool> VerificarUsuario(string pEmail, string pContra);
        Task<int> ObtenerUsuarioIDToken();
        Task<string> ObtenerUsuarioEstadoToken();
        Task<int> ObtenerUsuarioIDRefreshToken(string refreshToken);
        Task<string> ObtenerUsuarioRoleToken();
        Task<string> ObtenerRefreshToken(int pUsuario);
        string GenerarTokenAcceso(UsuarioSalida usuario);
        Task<string> GenerarYGuardarRefreshToken(int userId, string userRole);
        Task EliminarRefreshToken(int userId);
        Task<bool> EsRefreshTokenValido(string refreshToken);
        void EliminarCookie(string pNombreCookie);
        public void ActualizarCookie(string refreshToken);
    }
}
