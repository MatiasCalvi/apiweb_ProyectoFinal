using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Datos.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class PublicacionServiciosController : ControllerBase
    {
        private readonly IPublicacionServicios _publicacionServicios;
        private readonly IMetodosDeValidacion _metodosDeValidacion;

        public PublicacionServiciosController(IPublicacionServicios publicacionServicios, IMetodosDeValidacion metodosDeValidacion)
        {
            _publicacionServicios = publicacionServicios;
            _metodosDeValidacion = metodosDeValidacion;
        }

        [HttpGet("ObtenerPublicaciones")]
        public async Task<IActionResult> ObtenerPublicaciones() 
        {
            try
            {
                List<PublicacionSalida> publicaciones = await _publicacionServicios.ObtenerPublicaciones();
                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la busqueda", Detalle = ex.Message });
            }
        }

        [HttpGet("ObtenerPublicacion")]
        public async Task<IActionResult> ObtenerPublicacion([FromQuery] int idPublicacion) 
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(idPublicacion);

                if (publicacion == null) return NotFound(new { Mensaje = "Publicacion no encontrada" });

                return Ok(publicacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la busqueda", Detalle = ex.Message });
            }
        }

        [HttpGet("PublicacionesDeUnUsuario")] //--> esto le sirve al usuario para ver sus publicaciones
        [Authorize]
        public async Task<IActionResult> PublicacionesDeUnUsuario()
        {
            try
            {
                int usuarioIdClaim = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                List<PublicacionSalida> publicaciones = await _publicacionServicios.PublicacionesDeUnUsuario(usuarioIdClaim);
                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la busqueda", Detalle = ex.Message });
            }
        }

        [HttpPost("CrearPublicacion")]
        [Authorize]
        public async Task<IActionResult> CrearPublicacion([FromBody] PublicacionCreacion publicacion)
        {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                PublicacionSalidaC publicacionSalida = await _publicacionServicios.CrearPublicacion(publicacion);

                if (publicacionSalida != null)
                {
                    return CreatedAtAction(nameof(ObtenerPublicacion), new { id = publicacionSalida.Public_ID }, publicacionSalida);
                }

                return BadRequest(new { Mensaje = "Error al crear la publicación." }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Mensaje = "Error interno del servidor.", Detalle = ex.Message });
            }
        }

        [HttpPatch("EditarPublicacion")]
        [Authorize]

        public async Task<IActionResult> EditarPublicacion([FromQuery]int publicacionID,[FromBody] PublicacionModif publicacionEntrada) // el Admin deberia poder modificar el UsuarioID de la publicacion cosa que un usuario no puede hacer
        {
            try
            {
                if (publicacionEntrada == null )
                {
                    return BadRequest(new { Mensaje = "No se proporcionaron datos válidos para la actualización." });
                }

                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);
                
                if (publicacion == null) return BadRequest(new { Mensaje = "Publicacion no encontrada" });

                bool publicacionModif = await _publicacionServicios.EditarPublicacion(publicacionID, publicacionEntrada);

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la actualización de la publicacion", Detalle = ex.Message });
            }
        }

        [HttpPatch("PausarPublicacion")]
        [Authorize]
        public async Task<IActionResult> PausarPublicacion([FromQuery] int publicacionID) 
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                bool yaPausada = await _publicacionServicios.VerificarPublicPausada(publicacionID);

                if (publicacion == null || yaPausada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta pausada" });
                }

                bool resultado = await _publicacionServicios.PausarPublicacion(publicacionID, usuarioId);
                
                if (!resultado) return Forbid();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la pausa de la publicacion", Detalle = ex.Message });
            }
        }

        [HttpPatch("CancelarPublicacion")]
        [Authorize]
        public async Task<IActionResult> CancelarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);
                
                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                bool yaCancelada = await _publicacionServicios.VerificarPublicCancelada(publicacionID);
                
                if (publicacion == null || yaCancelada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta cancelada" });
                }

                bool resultado = await _publicacionServicios.CancelarPublicacion(publicacionID,usuarioId);

                if (!resultado) return Forbid();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la pausa de la publicacion", Detalle = ex.Message });
            }
        }

        [HttpPatch("ActivarPublicacion")]
        [Authorize]
        public async Task<IActionResult> ActivarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);
                
                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                bool yaActivada = await _publicacionServicios.VerificarPublicActivada(publicacionID);

                if (publicacion == null || yaActivada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya se esta activada" });
                }

                bool resultado = await _publicacionServicios.ActivarPublicacion(publicacionID, usuarioId);
                
                if (!resultado) return Forbid();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Msj = "Error durante la pausa de la publicacion", Detalle = ex.Message });
            }
        }
    }
}

