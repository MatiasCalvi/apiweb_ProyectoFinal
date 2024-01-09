using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;

namespace Datos.Servicios
{
    public class HistoriaServicios : IHistoriaServicios
    {
        private IDaoBDHistorias _daoBDHistorias;
        public HistoriaServicios(IDaoBDHistorias daoBDHistorias)
        {
            _daoBDHistorias = daoBDHistorias;
        }

        public async Task<List<HistoriaCompraSalida>> ObtenerHistorial(int pUsuarioID)
        {
            return await _daoBDHistorias.ObtenerHistorial(pUsuarioID);
        }
    }
}
