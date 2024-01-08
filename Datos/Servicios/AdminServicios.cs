using Datos.Exceptions;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Servicios
{
    public class AdminServicios : IAdminServicios
    {
        private IDaoBDAdmins _daoBDAdmins;
        private IPublicacionServicios _publicacionServicios;
        private IOfertasServicios _ofertasServicios;

        public AdminServicios(IDaoBDAdmins daoBDAdmins, IPublicacionServicios publicacionServicios, IOfertasServicios ofertasServicios)
        {
            _daoBDAdmins = daoBDAdmins;
            _publicacionServicios = publicacionServicios;
            _ofertasServicios = ofertasServicios;
        }

        public async Task<List<UsuarioSalida>> ObtenerTodosLosUsuarios()
        {
            return await _daoBDAdmins.ObtenerTodosLosUsuarios();
        }

        public async Task<List<PublicacionSalida>> ObtenerPublicaciones()
        {
            List<PublicacionSalida> lista = await _daoBDAdmins.ObtenerPublicaciones();
            List<PublicacionSalida> nuevalista = new List<PublicacionSalida>();
            int descuento;

            foreach (PublicacionSalida publicacion in lista)
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

        public async Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID)
        {
            List<PublicacionSalida> lista = await _daoBDAdmins.PublicacionesDeUnUsuario(pUsuarioID); 
            List<PublicacionSalida> nuevalista = new List<PublicacionSalida>();
            int descuento;

            foreach (PublicacionSalida publicacion in lista)
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

        public async Task<List<CarritoSalida>> ObtenerCarritos()
        {
            List<CarritoSalida> lista = await _daoBDAdmins.ObtenerCarritos();
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

        public async Task<List<HistoriaCompraSalida>> ObtenerHistoriales()
        {
            return await _daoBDAdmins.ObtenerHistoriales();
        }

        public async Task<List<OfertaSalida>> ObtenerOfertas()
        {
            return await _daoBDAdmins.ObtenerTodasLasOfertas();
        }

        public async Task<bool> VerificarUsuarioHabilitado(int pId)
        {
            bool resultado = await _daoBDAdmins.VerificarUsuarioHabilitado(pId);

            return resultado;
        }

        public async Task<bool> VerificarUsuarioDeshabilitado(int pId)
        {
            bool resultado = await _daoBDAdmins.VerificarUsuarioDeshabilitado(pId);
            
            return resultado;
        }

        public async Task<bool> HabilitarUsuario(int pId)
        {
            bool resultado = await _daoBDAdmins.HabilitarUsuario(pId);

            if (!resultado)
            {
                throw new DeletionFailedException($"No se pudo habilitar el usuario con ID {pId}.");
            }

            return resultado;
        }

        public async Task<bool> DeshabilitarUsuario(int pId)
        {
            bool resultado = await _daoBDAdmins.DeshabilitarUsuario(pId);

            if (!resultado)
            {
                throw new DeletionFailedException($"No se pudo deshabilitar la usuario con ID {pId}.");
            }

            return resultado;       
        }

        public async Task<bool> AsignarRolAAdmin(int pId)
        {
            bool resultado = await _daoBDAdmins.AsignarRolAAdmin(pId);

            if (!resultado)
            {
                throw new UpdateFailedException($"No se pudo cambiar el rol del usuario con ID {pId} a admin.");
            }

            return resultado;
        }

        public async Task<bool> AsignarRolAUsuario(int pId)
        {
            bool resultado = await _daoBDAdmins.AsignarRolAUsuario(pId);

            if (!resultado)
            {
                throw new UpdateFailedException($"No se pudo asignar el rol 'usuario' al usuario con ID {pId}.");
            }

            return resultado;
        }

        public async Task<bool> EditarPublicacion(int pId, PublicacionModifA pPublicacionModif)
        {
            PublicacionSalidaM publicacionActual = await _publicacionServicios.ObtenerPublicacionPorIDM(pId);
            DateTime fechaActual = DateTime.Now;

            publicacionActual.Public_UsuarioID = pPublicacionModif.Public_UsuarioID ?? publicacionActual.Public_UsuarioID;
            publicacionActual.Public_Nombre = pPublicacionModif.Public_Nombre ?? publicacionActual.Public_Nombre;
            publicacionActual.Public_Descripcion = pPublicacionModif.Public_Descripcion ?? publicacionActual.Public_Descripcion;
            publicacionActual.Public_Precio = pPublicacionModif.Public_Precio ?? publicacionActual.Public_Precio;
            publicacionActual.Public_Imagen = pPublicacionModif.Public_Imagen ?? publicacionActual.Public_Imagen;
            publicacionActual.Public_Stock = pPublicacionModif.Public_Stock ?? publicacionActual.Public_Stock;

            PublicacionModifA publicacion = new PublicacionModifA
            {
                Public_UsuarioID = publicacionActual.Public_UsuarioID,
                Public_Nombre = publicacionActual.Public_Nombre,
                Public_Descripcion = publicacionActual.Public_Descripcion,
                Public_Precio = publicacionActual.Public_Precio,
                Public_Imagen = publicacionActual.Public_Imagen,
                Public_Stock = publicacionActual.Public_Stock,
                Public_FModif = fechaActual
            };

            bool actualizado = await _daoBDAdmins.EditarPublicacion(pId, publicacion);

            return actualizado;
        }

        public async Task<bool> EditarOfertaAdmin(int pId, OfertaModifA pOfertaModif)
        {
            DateTime fechaActual = DateTime.Now;
            pOfertaModif.Oferta_FModif = fechaActual;

            bool actualizado = await _daoBDAdmins.EditarOfertaAdmin(pId, pOfertaModif);

            return actualizado;
        }
    }
}
