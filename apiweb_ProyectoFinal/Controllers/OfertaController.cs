using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;
using Datos.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace apiweb_ProyectoFinal.Controllers
{
    [ApiController]
    [Route("Ofertas")]
    public class OfertaController : Controller
    {
        private IMetodosDeValidacion _metodosDeValidacion;
        private IOfertasServicios _ofertasServicios;

        private readonly ILogger<OfertaController> _logger;
        public OfertaController(ILogger<OfertaController> logger, IMetodosDeValidacion metodosDeValidacion, IOfertasServicios ofertasServicios)
        {
            _logger = logger;
            _metodosDeValidacion = metodosDeValidacion;
            _ofertasServicios = ofertasServicios;
        }

        [HttpGet("ObtenerOfertas")]
        public async Task<IActionResult> ObtenerOfertas()
        {
            try
            {
                List<OfertaSalida> ofertas = await _ofertasServicios.ObtenerOfertas();

                return Ok(ofertas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Al obtener las ofertas");
                return StatusCode(500);
            }
        }

        [HttpGet("ObtenerOferta")] 
        public async Task<IActionResult> ObtenerOferta([FromQuery] int idOferta)
        {
            try
            {
                OfertaSalida oferta = await _ofertasServicios.ObtenerOfertaPorID(idOferta);

                if (oferta == null) return NotFound(new { Mensaje = "Oferta no encontrada" });

                return Ok(oferta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error Al obtener la oferta");
                return StatusCode(500);
            }
        }

        [HttpGet("TusOfertas")]
        [Authorize]
        public async Task<IActionResult> TusOfertas()
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                List<OfertaSalida> ofertas = await _ofertasServicios.ObtenerOfertasPorUsuarioID(usuarioID);

                if (ofertas == null) return NotFound(new { Mensaje = "No se encontraron Ofertas" });

                return Ok(ofertas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Al obtener sus ofertas");
                return StatusCode(500);
            }
        }

        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Crear([FromBody] OfertaCreacion oferta)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                OfertaSalidaC ofertaSalida = await _ofertasServicios.CrearOferta(oferta);

                return CreatedAtAction(nameof(ObtenerOferta), new { id = ofertaSalida.Oferta_ID }, ofertaSalida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Al obtener sus ofertas");
                return StatusCode(500);
            }
        }

        [HttpPatch("Editar")]
        [Authorize]
        public async Task<IActionResult> Editar([FromQuery] int ofertaID, [FromBody] OfertaModif ofertaEntrada)
        {
            try
            {
                OfertaSalida oferta = await _ofertasServicios.ObtenerOfertaPorID(ofertaID);

                if (oferta == null) return NotFound(new { Mensaje = "Oferta no encontrada" });
                
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                bool autoria = await _ofertasServicios.VerificarAutoria(usuarioID, ofertaEntrada.Oferta_ProdOfer);

                if ( (usuarioID != oferta.Oferta_UsuarioID) || !autoria) return Forbid();

                bool ofertaModif = await _ofertasServicios.EditarOferta(ofertaID, ofertaEntrada);

                if (!ofertaModif) return BadRequest(new { Mensaje = "Ah ocurrido un error al intentar editar la oferta" });

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar la oferta");
                return StatusCode(500);
            }
        }

        [HttpPatch("QuitarProductos")]
        [Authorize]
        public async Task<IActionResult> QuitarProductos([FromQuery] int ofertaID)
        {
            try
            {
                OfertaSalida oferta = await _ofertasServicios.ObtenerOfertaPorID(ofertaID);

                if (oferta == null)
                {
                    return NotFound(new { Mensaje = $"Oferta con ID: {ofertaID} no encontrada" });
                }

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuarioId != oferta.Oferta_UsuarioID) return Forbid();

                bool resultado = await _ofertasServicios.DesasociarPublicaciones(ofertaID);

                if (!resultado) return BadRequest();

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la oferta");
                return StatusCode(500);
            }
        }

        [HttpDelete("Eliminar")]
        [Authorize]
        public async Task<IActionResult> Eliminar([FromQuery] int ofertaID)
        {
            try
            {
                OfertaSalida oferta = await _ofertasServicios.ObtenerOfertaPorID(ofertaID);
                
                if (oferta == null )
                {
                    return NotFound(new { Mensaje = $"Oferta con ID: {ofertaID} no encontrada" });
                }

                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                if (usuarioId != oferta.Oferta_UsuarioID) return Forbid();

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

        [HttpDelete("EliminarTodo")]
        [Authorize]
        public async Task<IActionResult> EliminarTodo()
        {
            try
            {
                int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                bool resultado = await _ofertasServicios.EliminarOfertas(usuarioId);

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