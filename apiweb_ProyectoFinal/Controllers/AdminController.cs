using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Datos.Servicios;
using Datos.Validaciones;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiweb_ProyectoFinal.Controllers
{
    [ApiController]
    [Route("Admin")]
    [Authorize(Roles = "2")]
    
    public class AdminController : Controller
    {
        private IUsuarioServicios _usuarioServicios;
        private IAdminServicios _adminServicios;
        private IPublicacionServicios _publicacionServicios;

        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger, IUsuarioServicios usuarioServicios, IAdminServicios adminServicios, IPublicacionServicios publicacionServicios)
        {
            _logger = logger;
            _usuarioServicios = usuarioServicios;
            _adminServicios = adminServicios;
            _publicacionServicios = publicacionServicios;
        }

        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            try
            {
                List<UsuarioSalida> usuario = await _adminServicios.ObtenerTodosLosUsuarios();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpGet("ObtenerPublicaciones")]
        public async Task<IActionResult> ObtenerPublicaciones()
        {
            try
            {
                List<PublicacionSalida> publicaciones = await _adminServicios.ObtenerPublicaciones();
                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la busqueda", Detalle = ex.Message });
            }
        }

        [HttpGet("ObtenerCarritos")]
        public async Task<IActionResult> ObtenerCarritos()
        {
            try
            {
                List<CarritoSalida> lista = await _adminServicios.ObtenerCarritos();

                return Ok(lista);

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpGet("ObtenerHistoriales")]
        public async Task<IActionResult> ObtenerHistoriales()
        {
            try
            {
                List<HistoriaCompraSalida> historiales = await _adminServicios.ObtenerHistoriales();

                return Ok(historiales);
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("HabilitarUsuario")]
        public async Task<IActionResult> HabilitarUsuario([FromQuery] int id)
        {
            try
            {
                UsuarioSalida usuarioValido = await _usuarioServicios.ObtenerUsuarioPorID(id);

                bool yaHabilitado = await _adminServicios.VerificarUsuarioHabilitado(id);

                if (usuarioValido == null || yaHabilitado) return NotFound(new { Mensaje = $"Usuario con ID {id} no encontrado o ya habilitado." });

                bool resultado = await _adminServicios.HabilitarUsuario(id);

                return NoContent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("DeshabilitarUsuario")]
        public async Task<IActionResult> DeshabilitarUsuario([FromQuery] int id)
        {
            try
            {   
                UsuarioSalida usuarioValido = await _usuarioServicios.ObtenerUsuarioPorID(id);

                bool yaDeshabilitado = await _adminServicios.VerificarUsuarioDeshabilitado(id);
                
                if(usuarioValido == null || yaDeshabilitado) return NotFound(new { Mensaje = $"Usuario con ID {id} no encontrado o ya deshabilitado." });

                bool resultado = await _adminServicios.DeshabilitarUsuario(id);

                return NoContent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("AsignarAdmin")]
        public async Task<IActionResult> AsignarRolAAdmin([FromQuery] int id)
        {
            try
            {
                UsuarioSalida usuarioValido = await _usuarioServicios.ObtenerUsuarioPorID(id);

                if (usuarioValido == null)
                {
                    return NotFound(new { Mensaje = $"Usuario con ID {id} no encontrado." });
                }

                bool resultado = await _adminServicios.AsignarRolAAdmin(id);
                
                return NoContent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("AsignarRolAUsuario")]
        public async Task<IActionResult> AsignarRolAUsuario([FromQuery] int id)
        {
            try
            {
                UsuarioSalida usuarioValido = await _usuarioServicios.ObtenerUsuarioPorID(id);

                if (usuarioValido == null)
                {
                    return NotFound(new { Mensaje = $"Usuario con ID {id} no encontrado." });
                }

                bool resultado = await _adminServicios.AsignarRolAUsuario(id);
                
                return NoContent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("EditarPublicacion")]
        public async Task<IActionResult> EditarPublicacion([FromQuery] int publicacionID, [FromBody] PublicacionModifA publicacionEntrada) 
        {
            try
            {
                if (publicacionEntrada == null)
                {
                    return BadRequest(new { Mensaje = "No se proporcionaron datos válidos para la actualización." });
                }

                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                if (publicacion == null) return BadRequest(new { Mensaje = "Publicacion no encontrada" });

                bool publicacionModif = await _adminServicios.EditarPublicacion(publicacionID, publicacionEntrada);
                
                return NoContent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("PausarPublicacion")]
        public async Task<IActionResult> PausarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                bool yaPausada = await _publicacionServicios.VerificarPublicPausada(publicacionID);

                if (publicacion == null || yaPausada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta pausada" });
                }

                bool resultado = await _publicacionServicios.PausarPublicacionAdmin(publicacionID);

                return NoContent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("CancelarPublicacion")]
        public async Task<IActionResult> CancelarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                bool yaCancelada = await _publicacionServicios.VerificarPublicCancelada(publicacionID);

                if (publicacion == null || yaCancelada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta cancelada" });
                }

                bool resultado = await _publicacionServicios.CancelarPublicacionAdmin(publicacionID);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPatch("ActivarPublicacion")]
        public async Task<IActionResult> ActivarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                bool yaActivada = await _publicacionServicios.VerificarPublicActivada(publicacionID);

                if (publicacion == null || yaActivada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya se esta activada" });
                }

                bool resultado = await _publicacionServicios.ActivarPublicacionAdmin(publicacionID);

                return NoContent();

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }
    }
}
