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
    public class OfertasController : Controller
    {
        private IUsuarioServicios _usuarioServicios;
        private IMetodosDeValidacion _metodosDeValidacion;
        private IPublicacionServicios _publicacionServicios;
        private IOfertasServicios _ofertasServicios;

        private readonly ILogger<OfertasController> _logger;
        public OfertasController(ILogger<OfertasController> logger, IUsuarioServicios usuarioServicios, IMetodosDeValidacion metodosDeValidacion, IPublicacionServicios publicacionServicios, IOfertasServicios ofertasServicios)
        {
            _logger = logger;
            _usuarioServicios = usuarioServicios;
            _metodosDeValidacion = metodosDeValidacion;
            _publicacionServicios = publicacionServicios;
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
                _logger.LogError(ex, "Error Al obtener la oferta");
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
        public async Task<IActionResult> CrearOferta([FromBody] OfertaCreacion oferta)
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

        [HttpPatch("EditarOferta")]
        [Authorize]

        public async Task<IActionResult> EditarOferta([FromQuery] int ofertaID, [FromBody] OfertaModif ofertaEntrada)
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
    }
}