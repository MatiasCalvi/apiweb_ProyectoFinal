using Datos.Interfaces.IServicios;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiweb_ProyectoFinal.Controllers
{
    [ApiController]
    [Route("Admin")]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IUsuarioServicios _usuarioServicios;
        private IAdminServicios _adminServicios;
        private IPublicacionServicios _publicacionServicios;
        private IOfertasServicios _ofertasServicios;

        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger, IUsuarioServicios usuarioServicios, IAdminServicios adminServicios, IPublicacionServicios publicacionServicios, IOfertasServicios ofertasServicios)
        {
            _logger = logger;
            _usuarioServicios = usuarioServicios;
            _adminServicios = adminServicios;
            _publicacionServicios = publicacionServicios;
            _ofertasServicios = ofertasServicios;
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
                _logger.LogError(ex, "Error al obtener los usuarios");
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
                _logger.LogError(ex, "Error al obtener las publicaciones");
                return StatusCode(500);
            }
        }

        [HttpGet("PublicacionesUsuario")]
        public async Task<IActionResult> PublicacionesUsuario([FromQuery]int userID)
        {
            try
            {
                List<PublicacionSalida> publicaciones = await _adminServicios.PublicacionesDeUnUsuario(userID);
                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las publicaciones");
                return StatusCode(500);
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
                _logger.LogError(ex, "Error al obtener los carritos");
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
                _logger.LogError(ex, "Error al obtener los historiales");
                return StatusCode(500);
            }
        }

        [HttpGet("ObtenerOfertas")]
        public async Task<IActionResult> ObtenerOfertas()
        {
            try
            {
                List<OfertaSalida> ofertas = await _adminServicios.ObtenerOfertas();

                return Ok(ofertas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Al obtener las ofertas");
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
                _logger.LogError(ex, $"Error al habilitar el usuario: {id}");
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
                _logger.LogError(ex.Message);
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
                _logger.LogError(ex, $"Error al asignar el rol admin al usuario: {id}");
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
                _logger.LogError(ex, $"Error al asignar el rol al usuario: {id}");
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
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPatch("PausarPublicacion")]
        public async Task<IActionResult> PausarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                bool yaPausada = await _publicacionServicios.VerificarPublicEstado(publicacionID,4);

                if (publicacion == null || yaPausada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta pausada" });
                }

                bool resultado = await _publicacionServicios.CambiarEstadoPublicacion(publicacionID,4);

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al pausar la publicacion: {publicacionID}");
                return StatusCode(500);
            }
        }

        [HttpPatch("CancelarPublicacion")]
        public async Task<IActionResult> CancelarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                bool yaCancelada = await _publicacionServicios.VerificarPublicEstado(publicacionID,5);

                if (publicacion == null || yaCancelada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya esta cancelada" });
                }

                bool resultado = await _publicacionServicios.CambiarEstadoPublicacion(publicacionID,5);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPatch("ActivarPublicacion")]
        public async Task<IActionResult> ActivarPublicacion([FromQuery] int publicacionID,[FromBody] PublicacionRelanzada nuevoStock)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                bool yaActivada = await _publicacionServicios.VerificarPublicEstado(publicacionID,3);

                if (publicacion == null || yaActivada)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID {publicacionID} no encontrada o ya se esta activada" });
                }

                bool resultado = await _publicacionServicios.ActivarPublicacion(publicacionID, nuevoStock.Public_Stock);

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Error al activar la publicacion: {publicacionID}");
                return StatusCode(500);
            }
        }

        [HttpPatch("EditarOferta")]
        public async Task<IActionResult> EditarOferta([FromQuery] int ofertaID, [FromBody] OfertaModif ofertaEntrada)
        {
            try
            {
                OfertaSalida oferta = await _ofertasServicios.ObtenerOfertaPorID(ofertaID);

                if (oferta == null) return NotFound(new { Mensaje = "Oferta no encontrada" });

                bool ofertaModif = await _adminServicios.EditarOfertaAdmin(ofertaID, ofertaEntrada);

                if (!ofertaModif) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar editar la oferta" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar la oferta");
                return StatusCode(500);
            }
        }

        [HttpPatch("CancelarOferta")]
        public async Task<IActionResult> CancelarOferta([FromQuery] int usuarioID, [FromBody] int ofertaID)
        {
            try
            {

                bool resultado = await _ofertasServicios.OfertaCancelar(usuarioID,ofertaID);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar cancelar la oferta" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar la oferta");
                return StatusCode(500);
            }
        }

        [HttpPatch("CancelarOfertas")]
        public async Task<IActionResult> CancelarOfertas([FromQuery] int usuarioID)
        {
            try
            {
                bool resultado = await _ofertasServicios.OfertasCancelar(usuarioID);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar cancelar las ofertas" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar las ofertas");
                return StatusCode(500);
            }
        }

        [HttpDelete("EliminarPublicacion")]
        public async Task<IActionResult> EliminarPublicacion([FromQuery] int publicacionID)
        {
            try
            {
                PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

                if (publicacion == null)
                {
                    return NotFound(new { Mensaje = $"Publicacion con ID: {publicacionID} no encontrada" });
                }

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

        [HttpDelete("EliminarPublicaciones")]
        public async Task<IActionResult> EliminarPublicaciones([FromBody]int usuarioID)
        {
            try
            {
                bool resultado = await _publicacionServicios.EliminarPublicaciones(usuarioID);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar eliminar las publicaciones" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la publicacion");
                return StatusCode(500);
            }
        }


        [HttpDelete("EliminarOferta")]
        public async Task<IActionResult> Eliminar([FromQuery] int ofertaID)
        {
            try
            {
                OfertaSalida oferta = await _ofertasServicios.ObtenerOfertaPorID(ofertaID);

                if (oferta == null)
                {
                    return NotFound(new { Mensaje = $"Oferta con ID: {ofertaID} no encontrada" });
                }

                bool resultado = await _ofertasServicios.EliminarOferta(ofertaID);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar eliminar la oferta" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la oferta");
                return StatusCode(500);
            }
        }

        [HttpDelete("EliminarOfertas")]
        public async Task<IActionResult> EliminarTodo([FromBody]int usuarioID)
        {
            try
            {
                bool resultado = await _ofertasServicios.EliminarOfertas(usuarioID);

                if (!resultado) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar eliminar las ofertas" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar las ofertas");
                return StatusCode(500);
            }
        }
    }
}
