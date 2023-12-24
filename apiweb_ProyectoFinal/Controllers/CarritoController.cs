using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;
using Datos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
                _logger.LogError(ex, "Error al obtener el carrito");
                return StatusCode(500);
            }
        }

        [HttpPost("AgregarAlCarrito")]
        public async Task<IActionResult> AgregarAlCarrito([FromBody] CarritoCreacion carrito)
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                PublicacionSalida publicacionExiste = await _publicacionServicios.ObtenerPublicacionPorID(carrito.Carrito_PID);
                if (publicacionExiste == null) return NotFound(new {Mensaje = "publicacion no encontrada"});

                if (publicacionExiste.Public_Stock < carrito.Carrito_ProdUnidades) return BadRequest(new { Mensaje = "Unidades insuficientes" }); 

                bool existe = await _carritoServicios.Duplicado(usuarioID, carrito.Carrito_PID);
                if (existe) return BadRequest(new {Mensaje = "ya existe dicha publicacion en su carrito" });

                bool resultado = await _carritoServicios.Agregar(usuarioID, carrito);

                if (resultado == null) return BadRequest(new { Mensaje = "No se pudo agregar al carrito" });

                return Ok(new { Msj = "Se agrego al carrito satisfactoriamente", Data = publicacionExiste });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar el producto: {carrito.Carrito_PID} al carrito");
                return StatusCode(500);
            }
        }

        [HttpPatch("Comprar")]
        public async Task<IActionResult> Comprar([FromBody] CarritoCreacion carrito)
        {
            try
            {
                PublicacionSalida publicacionExiste = await _publicacionServicios.ObtenerPublicacionPorID(carrito.Carrito_PID);
                
                if (publicacionExiste == null) return NotFound(new { Mensaje = "publicacion no encontrada" });

                bool stock = await _carritoServicios.ReducirStock(carrito.Carrito_PID, carrito.Carrito_ProdUnidades);

                if (!stock) return BadRequest(new{ Mensaje = "No hay stock" });

                bool compra = await _carritoServicios.AgregarAlHistorial(carrito.Carrito_PID, carrito.Carrito_ProdUnidades);

                if (!compra) return BadRequest(new {Mensaje = "No se ha podido efectuar la compra" });

                return Ok(new { Mensaje = "Se ha comprado satisfactoriamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al comprar el producto: {carrito.Carrito_PID}");
                return StatusCode(500);
            }
        }

        [HttpPatch("ComprarTodo")]
        public async Task<IActionResult> ComprarTodo()
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                List<CarritoSalida> carritoDetallado = await _carritoServicios.ObtenerCarrito(usuarioID);

                foreach (var carritoItem in carritoDetallado)
                {
                    bool stock = await _carritoServicios.ReducirStock(carritoItem.Carrito_PID, carritoItem.Carrito_ProdUnidades);

                    if (!stock) return BadRequest(new { Mensaje = $"No hay stock para el producto '{carritoItem.Publicacion.Public_Nombre}' en el carrito" });

                    bool compra = await _carritoServicios.AgregarAlHistorial(carritoItem.Carrito_PID, carritoItem.Carrito_ProdUnidades);

                    if (!compra) return BadRequest(new { Mensaje = $"No se ha podido efectuar la compra para el producto '{carritoItem.Publicacion.Public_Nombre}' en el carrito" });
                }

                return Ok(new { Mensaje = "Se ha comprado satisfactoriamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al comprar los productos");
                return StatusCode(500);
            }
        }


        [HttpDelete("Eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] CarritoElim carrito)
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();
                
                PublicacionSalida publicacionExiste = await _publicacionServicios.ObtenerPublicacionPorID(carrito.Carrito_PID);
                
                if (publicacionExiste == null) return NotFound(new { Mensaje = "publicacion no encontrada" });
                
                bool eliminar = await _carritoServicios.Eliminar(usuarioID, carrito.Carrito_PID);
                
                if (!eliminar) return BadRequest();

                return Ok(new { Mensaje = "El producto se ha eliminado satisfactoriamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar el producto: {carrito.Carrito_PID}");
                return StatusCode(500);
            }
        }

        [HttpDelete("VaciarCarrito")]
        public async Task<IActionResult> VaciarCarrito()
        {
            try
            {
                int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();

                bool eliminar = await _carritoServicios.EliminarTodo(usuarioID);

                if (!eliminar) return BadRequest();

                return Ok(new { Mensaje = "Se ha vaciado el carrito satisfactoriamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al vaciar el carrito");
                return StatusCode(500);
            }
        }
    }
}
