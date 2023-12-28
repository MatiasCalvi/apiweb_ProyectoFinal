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
        private IMetodosDeValidacion _metodosDeValidacion;

        public PublicacionServicios(IDaoBDPublicaciones daoBDPublicaciones,IMetodosDeValidacion metodosDeValidacion)
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

        public async Task<bool> ActivarPublicacion(int pId, int pNuevoStock)
        {
            int usuarioID = await _metodosDeValidacion.ObtenerUsuarioIDToken();
            
            PublicacionModif nuevoStock = new PublicacionModif
            {
                Public_Stock = pNuevoStock
            };
            bool stock = await _daoBDPublicaciones.EditarPublicacion(pId, nuevoStock);
            
            if(!stock) return false;
            
            bool resultado = await _daoBDPublicaciones.ActivarPublicacion(pId, usuarioID);

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
