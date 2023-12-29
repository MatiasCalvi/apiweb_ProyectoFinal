using Datos.Interfaces.IServicios;
using Datos.Interfaces.IDaos;
using Datos.Modelos;
using Datos.Modelos.DTO;
using Datos.Interfaces.IValidaciones;


namespace Datos.Servicios
{
    public class PublicacionServicios : IPublicacionServicios
    {
        private IDaoBDPublicaciones _daoBDPublicaciones;

        public PublicacionServicios(IDaoBDPublicaciones daoBDPublicaciones)
        {
            _daoBDPublicaciones = daoBDPublicaciones;
        }

        public async Task<List<PublicacionSalida>> ObtenerPublicaciones()
        {
            return await _daoBDPublicaciones.ObtenerPublicaciones();
        }

        public async Task<PublicacionSalida> ObtenerPublicacionPorID(int pId)
        {
            return await _daoBDPublicaciones.ObtenerPublicacionPorID(pId);
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
            return await _daoBDPublicaciones.Buscar(pPalabraClave);
        }

        public async Task<List<PublicacionSalida>> PublicacionesDeUnUsuario(int pUsuarioID)
        {
            return await _daoBDPublicaciones.PublicacionesDeUnUsuario(pUsuarioID);
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
