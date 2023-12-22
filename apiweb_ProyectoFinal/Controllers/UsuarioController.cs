using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Mvc;
using Datos.Interfaces.IServicios;
using Datos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Datos.Interfaces.IValidaciones;

namespace apiWeb_MVC.Controllers
{   
    [ApiController] //ROLES: 2 = admin , 1= usuario
    [Route("Usuario")]

    public class UsuarioController : ControllerBase
    {
        private IUsuarioServicios _usuarioServicios;
        private IMetodosDeValidacion _metodosDeValidacion;

        private readonly ILogger<UsuarioController> _logger;
        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioServicios usuarioServicios, IMetodosDeValidacion metodosDeValidacion)
        {
            _logger = logger;
            _usuarioServicios = usuarioServicios;
            _metodosDeValidacion = metodosDeValidacion;
        }

        [HttpGet("ObtenerUsuario")]
        [Authorize(Roles = "2")]
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
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPost("Registrarse")]

        public async Task<IActionResult> Registrarse([FromBody] UsuarioCreacion pUsuario)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                UsuarioSalida usuario = await _usuarioServicios.ObtenerUsuarioPorEmail(pUsuario.Usuario_Email);//podes hacer este metodo un boolean
                if (usuario != null) return BadRequest(new { Mensaje = "Email ya en uso." });

                UsuarioSalidaC usuarioSalida = await _usuarioServicios.CrearNuevoUsuario(pUsuario);

                if (usuarioSalida != null)
                {
                    return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuarioSalida.Usuario_ID }, usuarioSalida);
                }

                else return BadRequest(new { Mensaje = "Hubo un problema al crear el usuario." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("ActualizarUsuario")] // *--> no se puede cambiar el Rol con este controlador
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> ActualizarUsuario([FromBody] UsuarioModif usuario)
        {   
            try
            {
                int usuarioIdClaim = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuario == null)
                {
                    return BadRequest(new { Mensaje = "No se proporcionaron datos v�lidos para la actualizaci�n." });
                }

                bool usuarioActualizado = await _usuarioServicios.ActualizarUsuario(usuarioIdClaim, usuario);

                if (usuarioActualizado) return NoContent();

                else return BadRequest(new { Mensaje = "No se realizaron cambios. No se realiz� ninguna actualizaci�n." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }
    }
}
