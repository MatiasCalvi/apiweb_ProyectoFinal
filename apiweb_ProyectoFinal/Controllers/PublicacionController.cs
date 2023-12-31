﻿using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("Publicacion")]

    public class PublicacionController : ControllerBase
    {
        private readonly IPublicacionServicios _publicacionServicios;
        private readonly IMetodosDeValidacion _metodosDeValidacion;

        private readonly ILogger<PublicacionController> _logger;

        public PublicacionController(ILogger<PublicacionController>logger,IPublicacionServicios publicacionServicios, IMetodosDeValidacion metodosDeValidacion)
        {
            _logger = logger;
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
                _logger.LogError(ex, "Error al Obtener las publicaciones");
                return StatusCode(500);
            }
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string palabraClave)
        {
            try
            {
                List<PublicacionSalida> publicaciones = await _publicacionServicios.Buscar(palabraClave);

                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al Obtener las publicaciones");
                return StatusCode(500);
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
                _logger.LogError(ex, "Error al obtener la publicacion");
                return StatusCode(500);
            }
        }

        [HttpGet("TusPublicaciones")] 
        [Authorize]
        public async Task<IActionResult> TusPublicaciones()
        {
            try
            {
                int usuarioIdClaim = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                List<PublicacionSalida> publicaciones = await _publicacionServicios.PublicacionesDeUnUsuario(usuarioIdClaim);
                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al obtener sus publicaciones");
                return StatusCode(500);
            }
        }

        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Crear([FromBody] PublicacionCreacion publicacion)
        {
            try 
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();
                publicacion.Public_UsuarioID = usuarioId;

                PublicacionSalidaC publicacionSalida = await _publicacionServicios.CrearPublicacion(publicacion);

                if (publicacionSalida != null)
                {
                    return CreatedAtAction(nameof(ObtenerPublicacion), new { id = publicacionSalida.Public_ID }, publicacionSalida);
                }

                return BadRequest(new { Mensaje = "Error al crear la publicación." }); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Interno del Servidor");
                return StatusCode(500);
            }
        }

        [HttpPatch("Editar")]
        [Authorize]

        public async Task<IActionResult> Editar([FromQuery]int publicacionID,[FromBody] PublicacionModif publicacionEntrada) 
        {
            try
            {
                if (publicacionEntrada == null )
                {
                    return BadRequest(new { Mensaje = "No se proporcionaron datos válidos para la actualización." });
                }

                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                if (publicacion == null) return NotFound(new { Mensaje = "Publicacion no encontrada" });
                
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuarioID != publicacion.Public_UsuarioID) return Forbid();

                bool publicacionModif = await _publicacionServicios.EditarPublicacion(publicacionID, publicacionEntrada);

                if(!publicacionModif) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar editar la publicacion" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar la publicacion");
                return StatusCode(500);
            }
        }

        [HttpPatch("Pausar")]
        [Authorize]
        public async Task<IActionResult> Pausar([FromQuery] int publicacionID) 
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuarioId != publicacion.Public_UsuarioID) return Forbid();

                bool yaPausada = await _publicacionServicios.VerificarPublicEstado(publicacionID,4);

                if (publicacion == null || yaPausada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta pausada" });
                }

                bool resultado = await _publicacionServicios.CambiarEstadoPublicacion(publicacionID, 4);
                
                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar pausar la publicacion" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al Pausar la publicacion");
                return StatusCode(500);
            }
        }

        [HttpPatch("Cancelar")]
        [Authorize]
        public async Task<IActionResult> Cancelar([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuarioId != publicacion.Public_UsuarioID) return Forbid();

                bool yaCancelada = await _publicacionServicios.VerificarPublicEstado(publicacionID, 5);

                if (publicacion == null || yaCancelada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta cancelada" });
                }

                bool resultado = await _publicacionServicios.CambiarEstadoPublicacion(publicacionID, 5);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar cancelar la publicacion" });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar la publicacion");
                return StatusCode(500);
            }
        }

        [HttpPatch("Activar")]
        [Authorize]
        public async Task<IActionResult> Activar([FromQuery] int publicacionID,[FromBody] PublicacionRelanzada nuevoStock )
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuarioID != publicacion.Public_UsuarioID) return Forbid();

                bool yaActivada = await _publicacionServicios.VerificarPublicEstado(publicacionID,3);

                if (publicacion == null || yaActivada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya se encuentra activada" });
                }

                bool resultado = await _publicacionServicios.ActivarPublicacion(publicacionID, nuevoStock.Public_Stock);
                
                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar activar la publicacion"});

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar la publicacion");
                return StatusCode(500);
            }
        }

        [HttpDelete("Eliminar")]
        [Authorize]
        public async Task<IActionResult> Eliminar([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                if (publicacion == null)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID: {publicacionID} no encontrada" });
                }

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuarioId != publicacion.Public_UsuarioID) return Forbid();

                bool resultado = await _publicacionServicios.EliminarPublicacion(publicacionID);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar eliminar la publicacion" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la publicacion");
                return StatusCode(500);
            }
        }

        [HttpDelete("EliminarTodo")]
        [Authorize]
        public async Task<IActionResult> EliminarTodo()
        {
            try
            {

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                bool resultado = await _publicacionServicios.EliminarPublicaciones(usuarioId);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar eliminar las publicaciones" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la publicacion");
                return StatusCode(500);
            }
        }
    }
}

