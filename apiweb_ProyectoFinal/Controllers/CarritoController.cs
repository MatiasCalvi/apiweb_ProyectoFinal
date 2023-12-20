using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiweb_ProyectoFinal.Controllers
{
    [ApiController]
    [Route("Carrito")]
    [Authorize]
    public class CarritoController : Controller
    {
        private IMetodosDeValidacion _metodosDeValidacion;
        private IPublicacionServicios _publicacionServicios;
        private ICarritoServicios _carritoServicios;

        private readonly ILogger<CarritoController> _logger;
        public CarritoController(ILogger<CarritoController> logger,ICarritoServicios carritoServicios, IMetodosDeValidacion metodosDeValidacion, IPublicacionServicios publicacionServicios)
        {
            _logger = logger;
            _metodosDeValidacion = metodosDeValidacion;
            _publicacionServicios = publicacionServicios;
            _carritoServicios = carritoServicios;
        }
        
        [HttpGet("ObtenerCarrito")]
        public async Task<IActionResult> ObtenerCarrito()
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();
                List<CarritoSalida> lista = await _carritoServicios.ObtenerCarrito(usuarioID);

                return Ok(lista);

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        [HttpPost("AgregarAlCarrito")]
        public async Task<IActionResult> AgregarAlCarrito([FromBody] int idPublicacion)
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                PublicacionSalida publicacionExiste = await _publicacionServicios.ObtenerPublicacionPorID(idPublicacion);
                if (publicacionExiste == null) return NotFound(new {Mensaje = "publicacion no encontrada"});

                bool existe = await _carritoServicios.Duplicado(usuarioID, idPublicacion);
                if (existe) return BadRequest(new {Mensaje = "ya existe dicha publicacion en su carrito" });

                bool resultado = await _carritoServicios.Agregar(usuarioID, idPublicacion);

                if (resultado == null) return BadRequest(new { Mensaje = "No se pudo agregar al carrito" });

                return Ok(new { Msj = "Se agrego al carrito satisfactoriamente", Data = publicacionExiste });

            }
            catch (Exception ex)
            {
                Console.WriteLine(new { ErrorDetalle = ex.Message });
                return StatusCode(500);
            }
        }

        //[HttpPatch("Comprar")]
        //public async Task<IActionResult> Comprar([FromBody] int idPublicacion)
        //{
        //    try
        //    {
        //        int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();
                
        //        PublicacionSalida publicacionExiste = await _publicacionServicios.ObtenerPublicacionPorID(idPublicacion);
        //        if (publicacionExiste == null) return NotFound(new { Mensaje = "publicacion no encontrada" });

        //        bool resultado = await _carritoServicios.Comprar(usuarioID,idPublicacion);
                
        //        if (!resultado) return BadRequest();

        //        return Ok(new {Mensaje = "Se ha comprado satisfactoriamente"});
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(new { ErrorDetalle = ex.Message });
        //        return StatusCode(500);
        //    }
        //}
    }
}
