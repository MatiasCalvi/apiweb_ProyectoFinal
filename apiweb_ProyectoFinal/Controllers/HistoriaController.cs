using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiweb_ProyectoFinal.Controllers
{
    [ApiController]
    [Route("HistoriaUsuario")]
    [Authorize]
    public class HistoriaController : Controller
    {
        private IMetodosDeValidacion _metodosDeValidacion;
        private IHistoriaServicios _historiaServicios;
        private readonly ILogger<HistoriaController> _logger;
        public HistoriaController(ILogger<HistoriaController> logger, IHistoriaServicios historiaServicios, IMetodosDeValidacion metodosDeValidacion)
        {
            _logger = logger;
            _metodosDeValidacion = metodosDeValidacion;
            _historiaServicios = historiaServicios;
        }

        [HttpGet("ObtenerHistorial")]
        public async Task<IActionResult> ObtenerHistorial()
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();
                List<HistoriaCompraSalida> historial = await _historiaServicios.ObtenerHistorial(usuarioID);

                return Ok(historial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al obtener el historial");
                return StatusCode(500);
            }
        }
    }
}
