using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Mvc;
using Datos.Interfaces.IServicios;
using Datos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Datos.Interfaces.IValidaciones;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UsuarioServiciosController : ControllerBase
    {
        private IUsuarioServicios _usuarioServicios;
        private IMetodosDeValidacion _metodosDeValidacion;

        private readonly ILogger<UsuarioServiciosController> _logger;
        public UsuarioServiciosController(ILogger<UsuarioServiciosController> logger, IUsuarioServicios usuarioServicios, IMetodosDeValidacion metodosDeValidacion)
        {
            _logger = logger;
            _usuarioServicios = usuarioServicios;
            _metodosDeValidacion = metodosDeValidacion;
        }

        [HttpGet("ObtenerUsuario")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ObtenerUsuario([FromQuery] int id)
        {
            try
            {
                UsuarioSalida usuario = await _usuarioServicios.ObtenerUsuarioPorID(id);

                if (usuario == null) return NotFound(new { Mensaje = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la busqueda", Detalle = ex.Message });
            }
        }

        [HttpPost("Registrarse")]

        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioCreacion pUsuario)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                
                UsuarioSalida usuario = await _usuarioServicios.ObtenerUsuarioPorEmail(pUsuario.Usuario_Email);

                if (usuario != null) return BadRequest(new { Mensaje = "Email ya en uso." });

                bool esAdmin = await _usuarioServicios.EsAdministrador(pUsuario.Usuario_Email);

                pUsuario.Usuario_Role = esAdmin ? "admin" : "usuario";

                UsuarioSalidaC usuarioSalida = await _usuarioServicios.CrearNuevoUsuario(pUsuario);

                if (usuarioSalida != null)
                {
                    return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuarioSalida.Usuario_ID }, usuarioSalida);
                }

                else return BadRequest(new { Mensaje = "Hubo un problema al crear el usuario." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj= "Error durante la creacion del usuario", Detalle = ex.Message });
            }
        }

        [HttpPatch("ActualizarUsuario")] // *--> no se puede cambiar el Rol con este controlador
        [Authorize(Roles = "usuario,admin")]
        public async Task<IActionResult> ActualizarUsuario([FromBody] UsuarioModif usuario)
        {   
            try
            {
                int usuarioIdClaim = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuario == null)
                {
                    return BadRequest(new { Mensaje = "No se proporcionaron datos válidos para la actualización." });
                }

                bool usuarioActualizado = await _usuarioServicios.ActualizarUsuario(usuarioIdClaim, usuario);

                if (usuarioActualizado) return NoContent();

                else return BadRequest(new { Mensaje = "No se realizaron cambios. No se realizó ninguna actualización." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la actualización del usuario", Detalle = ex.Message });
            }
        }
    }
}
