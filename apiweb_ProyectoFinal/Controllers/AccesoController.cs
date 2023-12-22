using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiweb_ProyectoFinal.Controllers
{
    [ApiController]
    [Route("Acceso")]
    public class AccesoController : Controller
    {
        private IUsuarioServicios _usuarioServicios;
        private IMetodosDeValidacion _metodosDeValidacion;

        private readonly ILogger<AccesoController> _logger;
        public AccesoController(ILogger<AccesoController> logger, IUsuarioServicios usuarioServicios, IMetodosDeValidacion metodosDeValidacion)
        {
            _logger = logger;
            _usuarioServicios = usuarioServicios;
            _metodosDeValidacion = metodosDeValidacion;
        }

        [HttpPost("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] UsuarioLogin usuario)
        {
            try
            {
                UsuarioSalida usuarioSalida = await _metodosDeValidacion.VerificarUsuario(usuario.Usuario_Email, usuario.Usuario_Contra);

                if (usuarioSalida == null)
                {
                    return Unauthorized(new { Mensaje = "Email o contraseña no válidos." });
                }

                var token = _metodosDeValidacion.GenerarTokenAcceso(usuarioSalida);

                var refreshToken = await _metodosDeValidacion.GenerarYGuardarRefreshToken(usuarioSalida.Usuario_ID, usuarioSalida.Usuario_Role.ToString());

                return Ok(new { Token = token, RefreshToken = refreshToken, Msj = $"Bienvenido {usuarioSalida.Usuario_Nombre}" });

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPost("CerrarSesion")]
        [Authorize]
        public async Task<IActionResult> CerrarSesion()
        {
            try
            {
                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                await _metodosDeValidacion.EliminarRefreshToken(usuarioId);

                _metodosDeValidacion.EliminarCookie("RefreshToken");

                return Ok(new { Mensaje = "Cierre de sesión exitoso" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPost("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                int usuarioRoleClaim = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                string refreshToken = await _metodosDeValidacion.ObtenerRefreshToken(usuarioRoleClaim);

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { Mensaje = "El token de actualización no se encuentra en la base de datos." });
                }

                UsuarioSalida usuarioSalida = await _usuarioServicios.ObtenerUsuarioPorID(usuarioRoleClaim);

                string token = _metodosDeValidacion.GenerarTokenAcceso(usuarioSalida);

                var cookie = Request.Cookies["RefreshToken"];

                if (cookie == null)
                {
                    return Unauthorized(new { Mensaje = "Falta el token de actualización o no es válido." });
                }

                _metodosDeValidacion.ActualizarCookie(refreshToken, cookie);

                return Ok(new { Token = token, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }
    }
}
