using Datos.Interfaces.IServicios;
using Datos.Interfaces.IDaos;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Datos.Interfaces.IValidaciones;
using Datos.Interfaces.IModelos;


namespace Datos.Servicios
{
    public class PublicacionServicios : IPublicacionServicios
    {
        private IDaoBDPublicaciones _daoBDPublicaciones;
        private IOfertasServicios _ofertasServicios;    

        public PublicacionServicios(IDaoBDPublicaciones daoBDPublicaciones, IOfertasServicios ofertasServicios)
        {
            _daoBDPublicaciones = daoBDPublicaciones;
            _ofertasServicios = ofertasServicios;
        }

        public async Task<List<PublicacionSalida>> ObtenerPublicaciones()
        {
            List<PublicacionSalida> lista = await _daoBDPublicaciones.ObtenerPublicaciones();
            List<PublicacionSalida> nuevalista = new List<PublicacionSalida>();
            int descuento;

            foreach(PublicacionSalida publicacion in lista)
            {
                descuento = await _ofertasServicios.VerificarDescuento(publicacion.Public_ID);
                publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                if (descuento == 0)
                {
                    publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                    nuevalista.Add(publicacion);
                }
                else
                {
                    decimal porcentajeDescuento = descuento / 100m;
                    decimal complemento = 1 - porcentajeDescuento;
                    publicacion.Public_PrecioFinal = publicacion.Public_Precio * complemento;
                    nuevalista.Add(publicacion);
                }
            }
            return nuevalista;
        }

        public async Task<PublicacionSalida> ObtenerPublicacionPorID(int pId)
        {
            PublicacionSalida publicacion = await _daoBDPublicaciones.ObtenerPublicacionPorID(pId);
            int descuento = await _ofertasServicios.VerificarDescuento(pId);
            
            if (publicacion == null) return publicacion;

            if (descuento == 0) 
            {
                publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                return publicacion; 
            }
            else
            {
                decimal porcentajeDescuento = descuento / 100m;
                decimal complemento = 1 - porcentajeDescuento;
                publicacion.Public_PrecioFinal = publicacion.Public_Precio * complemento;
                return publicacion;
            }
        }

        public async Task<PublicacionSalidaM> ObtenerPublicacionPorIDM(int pId)
        {
            return await _daoBDPublicaciones.ObtenerPublicacionPorIDM(pId);
        }

        public async Task<PublicacionSalida> ObtenerStock(int pId)
        {
            return await _daoBDPublicaciones.ObtenerStock(pId);
        }

        public async Task<List<PublicacionSalida>>Buscar(string pPalabraClave)
        {
            List<PublicacionSalida> lista = await _daoBDPublicaciones.Buscar(pPalabraClave); 
            List<PublicacionSalida> nuevalista = new List<PublicacionSalida>();
            int descuento;

            foreach (PublicacionSalida publicacion in lista)
            {
                descuento = await _ofertasServicios.VerificarDescuento(publicacion.Public_ID);
                if (descuento == 0)
                {
                    publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                    nuevalista.Add(publicacion);
                }
                else
                {
                    decimal porcentajeDescuento = descuento / 100m;
                    decimal complemento = 1 - porcentajeDescuento;
                    publicacion.Public_PrecioFinal = publicacion.Public_Precio * complemento;
                    nuevalista.Add(publicacion);
                }
            }
            return nuevalista;
        }

        public async Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID)
        {
            List<PublicacionSalida> lista = await _daoBDPublicaciones.PublicacionesDeUnUsuario(pUsuarioID);
            List<PublicacionSalida> nuevalista = new List<PublicacionSalida>();
            int descuento;

            foreach (PublicacionSalida publicacion in lista)
            {
                descuento = await _ofertasServicios.VerificarDescuento(publicacion.Public_ID);
                if (descuento == 0)
                {
                    publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                    nuevalista.Add(publicacion);
                }
                else
                {
                    decimal porcentajeDescuento = descuento / 100m;
                    decimal complemento = 1 - porcentajeDescuento;
                    publicacion.Public_Precio = publicacion.Public_Precio * complemento;
                    nuevalista.Add(publicacion);
                }
            }
            return nuevalista;
        }

        public async Task<PublicacionSalidaC> CrearPublicacion(PublicacionCreacion pPublicacion)
        {   
            pPublicacion.Public_FCreacion = DateTime.Now;
            PublicacionSalidaC publicacionSalida = await _daoBDPublicaciones.CrearPublicacion(pPublicacion);

            return publicacionSalida;
        }

        public async Task<bool> ActivarPublicacion(int pId, int pNuevoStock)
        {   
            PublicacionModif nuevoStock = new PublicacionModif
            {
                Public_Stock = pNuevoStock
            };

            bool stock = await _daoBDPublicaciones.EditarPublicacion(pId, nuevoStock);
            
            if(!stock) return false;
            
            bool resultado = await _daoBDPublicaciones.CambiarEstadoPublicacion(pId, 3);

            return resultado;
        }
        public async Task<bool> VerificarPublicEstado(int pPublicID, int pEstado)
        {
            bool resultado = await _daoBDPublicaciones.VerificarPublicEstado(pPublicID, pEstado);

            return resultado;
        }

        public async Task<bool> CambiarEstadoPublicacion(int pId,int pEstadoID)
        {
            bool resultado = await _daoBDPublicaciones.CambiarEstadoPublicacion(pId, pEstadoID);
            return resultado;
        }

        public async Task<bool> EditarPublicacion(int pId, PublicacionModif pPublicacionModif)
        {
            DateTime fechaActual = DateTime.Now;
            pPublicacionModif.Public_FModif = fechaActual;

            bool actualizado = await _daoBDPublicaciones.EditarPublicacion(pId, pPublicacionModif);

            return actualizado;
        }

        public async Task<bool> EliminarPublicacion(int pPublicacionID)
        {
            bool resultado = await _daoBDPublicaciones.EliminarPublicacion(pPublicacionID, null);

            return resultado;
        }

        public async Task<bool> EliminarPublicaciones(int pUsuarioID)
        {
            bool resultado = await _daoBDPublicaciones.EliminarPublicacion(null, pUsuarioID);

            return resultado;
        }
    }
}
