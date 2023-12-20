using Datos.Interfaces.IDaos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos.DTO;
using System.Security.Cryptography;

namespace Datos.Servicios
{
    public class CarritoServicios : ICarritoServicios
    {
        private IDaoBDCarrito _daoBDCarrito;
        private IPublicacionServicios _publicacionServicios;
        public CarritoServicios(IDaoBDCarrito daoBDCarrito)
        {
            _daoBDCarrito = daoBDCarrito;
        }

        public async Task<List<CarritoSalida>> ObtenerCarrito(int pUsuarioID)
        {
            return await _daoBDCarrito.ObtenerCarrito(pUsuarioID);
        }

        public async Task<bool>Agregar(int pUsuarioID, int pId)
        {
            
            bool result = await _daoBDCarrito.Agregar(pUsuarioID, pId);

            return result;
        }

        public async Task<bool>Duplicado(int pUsuarioID, int pId)
        {
            bool result = await _daoBDCarrito.Duplicado(pUsuarioID, pId);

            return result;
        }

        //public async Task<bool>Comprar(int pUsuarioID, int pId)
        //{
        //    bool result = await _daoBDCarrito.Comprar(pUsuarioID, pId);

        //    return result;
        //}
    }
}
