using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Servicios
{
    public class CarritoServicios : ICarritoServicios
    {
        private IDaoBDCarrito _daoBDCarrito;
        private IMetodosDeValidacion _metodosDeValidacion;
        private IPublicacionServicios _publicacionServicios;
        private IOfertasServicios _ofertasServicios;
        public CarritoServicios(IDaoBDCarrito daoBDCarrito, IMetodosDeValidacion metodosDeValidacion, IPublicacionServicios publicacionServicios, IOfertasServicios ofertasServicios)
        {
            _daoBDCarrito = daoBDCarrito;
            _metodosDeValidacion = metodosDeValidacion;
            _publicacionServicios = publicacionServicios;
            _ofertasServicios = ofertasServicios;
        }

        public async Task<List<CarritoSalida>> ObtenerCarrito(int pUsuarioID)
        {
            List<CarritoSalida> lista = await _daoBDCarrito.ObtenerCarrito(pUsuarioID);
            List<CarritoSalida> nuevalista = new List<CarritoSalida>();
            int descuento;

            foreach (CarritoSalida carrito in lista)
            {
                descuento = await _ofertasServicios.VerificarDescuento(carrito.Publicacion.Public_ID);
                if (descuento == 0)
                {
                    carrito.Publicacion.Public_PrecioFinal = carrito.Publicacion.Public_Precio;
                    nuevalista.Add(carrito);
                }
                else
                {
                    decimal porcentajeDescuento = descuento / 100m;
                    decimal complemento = 1 - porcentajeDescuento;
                    carrito.Publicacion.Public_PrecioFinal = carrito.Publicacion.Public_Precio * complemento;
                    nuevalista.Add(carrito);
                }
            }
            return nuevalista;
        }

        public async Task<bool> Agregar(int pUsuarioID, CarritoCreacion pCarrito)
        {

            bool result = await _daoBDCarrito.Agregar(pUsuarioID, pCarrito);

            return result;
        }

        public async Task<bool> Duplicado(int pUsuarioID, int pId)
        {
            bool result = await _daoBDCarrito.Duplicado(pUsuarioID, pId);

            return result;
        }

        public async Task<bool> AgregarAlHistorial(int pId, int pUnidadesProducto)
        {
            int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();
            Historia historiaCompra = new Historia
            {
                HC_UsuarioID = usuarioID,
                HC_PID = pId,
                HC_Unidades = pUnidadesProducto,
                HC_PrecioFinal = await CalcularPrecioFinal(pId, pUnidadesProducto),
                HC_FechaCompra = DateTime.Now,
            };

            bool result = await _daoBDCarrito.AgregarAlHistorial(historiaCompra);

            if (!result) return false;

            bool eliminado = await _daoBDCarrito.Eliminar(usuarioID, pId);

            return eliminado;
        }

        private async Task<decimal> CalcularPrecioFinal(int pPublicacionID, int unidadesProducto)
        {   
            int descuento = await _ofertasServicios.VerificarDescuento(pPublicacionID);
            PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(pPublicacionID);
            
            if(descuento == 0) return publicacion.Public_Precio * unidadesProducto;

            decimal porcentajeDescuento = descuento / 100m;
            decimal complemento = 1 - porcentajeDescuento;
            decimal precioConDescuento = publicacion.Public_Precio * complemento;

            return precioConDescuento * unidadesProducto;
        }

        public async Task<bool> ReducirStock (int publicacionID, int unidades)
        {
            PublicacionSalida publicacion = await _publicacionServicios.ObtenerPublicacionPorID(publicacionID);

            if (publicacion.Public_Stock < unidades) return false;

            publicacion.Public_Stock -= unidades;

            PublicacionModif publicacionFinal = new PublicacionModif
            {
                Public_UsuarioID = publicacion.Public_UsuarioID,
                Public_Nombre = publicacion.Public_Nombre,
                Public_Descripcion = publicacion.Public_Descripcion,
                Public_Precio = publicacion.Public_Precio,
                Public_Imagen = publicacion.Public_Imagen,
                Public_FModif = DateTime.Now,
                Public_Stock = publicacion.Public_Stock
            };

            if (publicacion.Public_Stock == 0)
            {
                publicacionFinal.Public_Estado = 4;
                
                await _publicacionServicios.CambiarEstadoPublicacion(publicacionID,4);
                                                                                                
                await _publicacionServicios.EditarPublicacion(publicacionID, publicacionFinal);

                return true;
            }

            await _publicacionServicios.EditarPublicacion(publicacionID, publicacionFinal);


            return  true;
        }

        public async Task<bool> Eliminar(int pUsuarioID, int pId)
        {
            bool result = await _daoBDCarrito.Eliminar(pUsuarioID, pId);

            return result;
        }

        public async Task<bool> EliminarTodo(int pUsuarioID)
        {
            bool result = await _daoBDCarrito.EliminarTodo(pUsuarioID);

            return result;
        }
    }
}
