using Datos.Interfaces.IServicios;
using Datos.Interfaces.IDaos;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;
using System.Reflection.PortableExecutable;
using Datos.Exceptions;
using System.Net;
using System.Data;

namespace Datos.Servicios
{
    public class PublicacionServicios : IPublicacionServicios
    {
        private IDaoBDPublicaciones _daoBDPublicaciones;
        private IMetodosDeValidacion _metodosDeValidacion;

        public PublicacionServicios(IDaoBDPublicaciones daoBDPublicaciones, IMetodosDeValidacion metodosDeValidacion)
        {
            _daoBDPublicaciones = daoBDPublicaciones;
            _metodosDeValidacion = metodosDeValidacion;
        }

        public async Task<List<PublicacionSalida>> ObtenerPublicaciones()
        {
            return await _daoBDPublicaciones.ObtenerPublicaciones();
        }

        public async Task<PublicacionSalida> ObtenerPublicacionPorID(int pId)
        {
            return await _daoBDPublicaciones.ObtenerPublicacionPorID(pId);
        }

        internal async Task<PublicacionSalidaM> ObtenerPublicacionPorIDM(int pId)
        {
            return await _daoBDPublicaciones.ObtenerPublicacionPorIDM(pId);
        }

        public async Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID)
        {
            return await _daoBDPublicaciones.PublicacionesDeUnUsuario(pUsuarioID);
        }

        public async Task<PublicacionSalidaC> CrearPublicacion(PublicacionCreacion pPublicacion)
        {   
            int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();
            pPublicacion.Public_UsuarioID = usuarioId;
            pPublicacion.Public_FCreacion = DateTime.Now;

            PublicacionSalidaC publicacionSalida = await _daoBDPublicaciones.CrearPublicacion(pPublicacion);

            return publicacionSalida;
        }

        public async Task<bool> PausarPublicacion(int pId, int pUsuarioID)
        {
            bool resultado = await _daoBDPublicaciones.PausarPublicacion(pId, pUsuarioID);
            return resultado;
        }

        public async Task<bool> CancelarPublicacion(int pId,int pUsuarioID)
        {
            bool resultado = await _daoBDPublicaciones.CancelarPublicacion(pId, pUsuarioID);
            return resultado;
        }

        public async Task<bool> ActivarPublicacion(int pId, int pUsuarioID)
        {
            bool resultado = await _daoBDPublicaciones.ActivarPublicacion(pId, pUsuarioID);
            return resultado;
        }

        public async Task<bool> PausarPublicacionAdmin(int pId)
        {
            bool resultado = await _daoBDPublicaciones.PausarPublicacionAdmin(pId);
            return resultado;
        }

        public async Task<bool> CancelarPublicacionAdmin(int pId)
        {
            bool resultado = await _daoBDPublicaciones.CancelarPublicacionAdmin(pId);
            return resultado;
        }

        public async Task<bool> ActivarPublicacionAdmin(int pId)
        {
            bool resultado = await _daoBDPublicaciones.ActivarPublicacionAdmin(pId);
            return resultado;
        }

        public async Task<bool> VerificarPublicPausada(int pId)
        {
            bool resultado = await _daoBDPublicaciones.VerificarPublicPausada(pId);

            return resultado;
        }

        public async Task<bool> VerificarPublicCancelada(int pId)
        {
            bool resultado = await _daoBDPublicaciones.VerificarPublicCancelada(pId);

            return resultado;
        }

        public async Task<bool> VerificarPublicActivada(int pId)
        {
            bool resultado = await _daoBDPublicaciones.VerificarPublicActivada(pId);

            return resultado;
        }

        public async Task<bool> EditarPublicacion(int pId, PublicacionModif pPublicacionModif)
        {
            PublicacionSalidaM publicacionActual = await ObtenerPublicacionPorIDM(pId);
            DateTime fechaActual = DateTime.Now;

            publicacionActual.Public_Nombre = pPublicacionModif.Public_Nombre ?? publicacionActual.Public_Nombre;
            publicacionActual.Public_Descripcion = pPublicacionModif.Public_Descripcion ?? publicacionActual.Public_Descripcion;
            publicacionActual.Public_Precio = pPublicacionModif.Public_Precio ?? publicacionActual.Public_Precio;
            publicacionActual.Public_Imagen = pPublicacionModif.Public_Imagen ?? publicacionActual.Public_Imagen;
            publicacionActual.Public_Stock = pPublicacionModif.Public_Stock ?? publicacionActual.Public_Stock;

            PublicacionModif publicacion = new PublicacionModif
            {
                Public_Nombre = publicacionActual.Public_Nombre,
                Public_Descripcion = publicacionActual.Public_Descripcion,
                Public_Precio = publicacionActual.Public_Precio,
                Public_Imagen = publicacionActual.Public_Imagen,
                Public_Stock = publicacionActual.Public_Stock,
                Public_FModif = fechaActual
            };

            bool actualizado = await _daoBDPublicaciones.EditarPublicacion(pId, publicacion);

            return actualizado;
        }

        public async Task<bool> EditarPublicacionAdmin(int pId, PublicacionModifA pPublicacionModif)
        {
            PublicacionSalidaM publicacionActual = await ObtenerPublicacionPorIDM(pId);
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

            bool actualizado = await _daoBDPublicaciones.EditarPublicacionAdmin(pId, publicacion);

            return actualizado;
        }
    }
}
