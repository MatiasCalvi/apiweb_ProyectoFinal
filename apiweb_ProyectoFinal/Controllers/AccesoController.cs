using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

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
                bool result = await _metodosDeValidacion.VerificarUsuario(usuario.Usuario_Email, usuario.Usuario_Contra);

                if (!result) return Unauthorized(new { Mensaje = "Email o contraseña no válidos." });

                UsuarioSalida usuarioSalida = await _usuarioServicios.ObtenerUsuarioPorEmail(usuario.Usuario_Email);

                if (usuarioSalida.Usuario_Estado == "Deshabilitado") return BadRequest(new {Mensaje = "Su usuario esta dado de baja"});

                var token = _metodosDeValidacion.GenerarTokenAcceso(usuarioSalida);

                var refreshToken = await _metodosDeValidacion.GenerarYGuardarRefreshToken(usuarioSalida.Usuario_ID, usuarioSalida.Usuario_Role.ToString());

                return Ok(new { Token = token, RefreshToken = refreshToken, Msj = $"Bienvenido {usuarioSalida.Usuario_Nombre}" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Iniciar Sesion");
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
                _logger.LogError(ex, "Error al Cerrar Sesion");
                return StatusCode(500);
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                string refreshToken = Request.Cookies["RefreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { Mensaje = "Falta el token de actualización o no es válido." });
                }

                if (!await _metodosDeValidacion.EsRefreshTokenValido(refreshToken))
                {
                    return Unauthorized(new { Mensaje = "El token de actualización ha expirado o no es válido." });
                }

                int usuarioIDClaim = await _metodosDeValidacion.ObtenerUsuarioIDRefreshToken(refreshToken);
                
                string refreshTokenBD = await _metodosDeValidacion.ObtenerRefreshToken(usuarioIDClaim);

                if (string.IsNullOrEmpty(refreshTokenBD))
                {
                    return Unauthorized(new { Mensaje = "El token de actualización no se encuentra en la base de datos." }); 
                }
                
                UsuarioSalida usuarioSalida = await _usuarioServicios.ObtenerUsuarioPorID(usuarioIDClaim);
               
                string token = _metodosDeValidacion.GenerarTokenAcceso(usuarioSalida);
                
                _metodosDeValidacion.ActualizarCookie(refreshToken);
                
                return Ok(new { Token = token, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Refrescar el Token");
                return BadRequest(new { Mensaje = "Se produjo un error al refrescar el token de acceso" });
            }
        }
    }
}
