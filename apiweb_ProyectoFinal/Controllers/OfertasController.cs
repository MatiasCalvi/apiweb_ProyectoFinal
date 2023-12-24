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

        [HttpGet("ObtenerOferta")] // controlar que no pueda subir publicaciones pausadas o canceladas
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
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("TusOfertas")]
        public async Task<IActionResult> TusOfertas([FromQuery] int idOferta)
        {
            try
            {
                OfertaSalida oferta = await _ofertasServicios.ObtenerOfertaPorID(idOferta);

                if (oferta == null) return NotFound(new { Mensaje = "Oferta no encontrada" });

                return Ok(oferta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        //[HttpPost("Crear")]
        //[Authorize]
        //public async Task<IActionResult> CrearPublicacion([FromBody] PublicacionCreacion oferta)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid) return BadRequest(ModelState);

        //        int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();
        //        publicacion.Public_UsuarioID = usuarioId;

        //        PublicacionSalidaC publicacionSalida = await _publicacionServicios.CrearPublicacion(oferta);

        //        if (publicacionSalida != null)
        //        {
        //            return CreatedAtAction(nameof(ObtenerPublicacion), new { id = publicacionSalida.Public_ID }, publicacionSalida);
        //        }

        //        return BadRequest(new { Mensaje = "Error al crear la publicación." });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(500);
        //    }
        //}
    }
}
