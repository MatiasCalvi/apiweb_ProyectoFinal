using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;

namespace Datos.Servicios
{
    public class OfertasServicios : IOfertasServicios
    {
        private IDaoBDOfertas _daoBDOfertas;
        private IMetodosDeValidacion _metodosDeValidacion;

        public OfertasServicios(IDaoBDOfertas daoBDOfertas, IMetodosDeValidacion metodosDeValidacion)
        {
            _daoBDOfertas = daoBDOfertas;
            _metodosDeValidacion = metodosDeValidacion;
        }

        public async Task<OfertaSalida> ObtenerOfertaPorID(int pId)
        {
            return await _daoBDOfertas.ObtenerOfertaPorID(pId);
        }

    }
}
