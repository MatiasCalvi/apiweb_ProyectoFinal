using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;

namespace Datos.Servicios
{
    public class HistoriaServicios : IHistoriaServicios
    {
        private IDaoBDHistorias _daoBDHistorias;
        private IMetodosDeValidacion _metodosDeValidacion;
        private IPublicacionServicios _publicacionServicios;
        public HistoriaServicios(IDaoBDHistorias daoBDHistorias, IMetodosDeValidacion metodosDeValidacion, IPublicacionServicios publicacionServicios)
        {
            _daoBDHistorias = daoBDHistorias;
            _metodosDeValidacion = metodosDeValidacion;
            _publicacionServicios = publicacionServicios;
        }

        public async Task<List<HistoriaCompraSalida>> ObtenerHistorial(int pUsuarioID)
        {
            return await _daoBDHistorias.ObtenerHistorial(pUsuarioID);
        }
    }
}
