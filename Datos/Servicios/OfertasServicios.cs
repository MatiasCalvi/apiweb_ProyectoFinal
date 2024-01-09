using Datos.Interfaces.IDaos;
using Datos.Interfaces.IModelos;
using Datos.Interfaces.IServicios;
using Datos.Interfaces.IValidaciones;
using Datos.Modelos;
using Datos.Modelos.DTO;
using System.Data.Common;

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

        public async Task<List<OfertaSalida>> ObtenerOfertas()
        {
            DateTime fechaActual = DateTime.Now;
            List<OfertaSalida> lista = await _daoBDOfertas.ObtenerOfertas(fechaActual);
            List<OfertaSalida> nuevalista = new List<OfertaSalida>();
            int descuento;

            for (int i = 0; i < lista.Count; i++)
            {
                OfertaSalida oferta = lista[i];
                for (int j = 0; j < oferta.Oferta_ProdOfer.Count; j++)
                {
                    PublicacionSalida publicacion = oferta.Oferta_ProdOfer[j];
                    descuento = await VerificarDescuento(publicacion.Public_ID);
                    publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                    if (descuento == 0)
                    {
                        publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                    }
                    else
                    {
                        decimal porcentajeDescuento = descuento / 100m;
                        decimal complemento = 1 - porcentajeDescuento;
                        publicacion.Public_PrecioFinal = publicacion.Public_Precio * complemento;
                    }
                }
                nuevalista.Add(oferta);
            }
            return nuevalista;
        }

        public async Task<OfertaSalida> ObtenerOfertaPorID(int pId)
        {
            OfertaSalida oferta = await _daoBDOfertas.ObtenerOfertaPorID(pId);

            if (oferta != null && oferta.Oferta_ProdOfer != null)
            {
                int descuento;

                foreach (PublicacionSalida publicacion in oferta.Oferta_ProdOfer)
                {
                    descuento = await VerificarDescuento(publicacion?.Public_ID ?? 0);
                    publicacion.Public_PrecioFinal = publicacion.Public_Precio;

                    if (descuento != 0)
                    {
                        decimal porcentajeDescuento = descuento / 100m;
                        decimal complemento = 1 - porcentajeDescuento;
                        publicacion.Public_PrecioFinal = publicacion.Public_Precio * complemento;
                    }
                }
            }

            return oferta;
        }

        public async Task<List<OfertaSalida>> ObtenerOfertasPorUsuarioID(int pId)
        {
            List<OfertaSalida> lista = await _daoBDOfertas.ObtenerOfertasPorUsuarioID(pId);
            List<OfertaSalida> nuevalista = new List<OfertaSalida>();
            int descuento;

            foreach (OfertaSalida oferta in lista)
            {
                foreach (PublicacionSalida publicacion in oferta.Oferta_ProdOfer)
                {
                    if (publicacion != null)
                    {
                        descuento = await VerificarDescuento(publicacion?.Public_ID ?? 0);
                        publicacion.Public_PrecioFinal = publicacion.Public_Precio;

                        if (descuento == 0)
                        {
                            publicacion.Public_PrecioFinal = publicacion.Public_Precio;
                        }
                        else
                        {
                            decimal porcentajeDescuento = descuento / 100m;
                            decimal complemento = 1 - porcentajeDescuento;
                            publicacion.Public_PrecioFinal = publicacion.Public_Precio * complemento;
                        }
                    }
                }

                nuevalista.Add(oferta);
            }

            return nuevalista;
        }

        public async Task<bool> VerificarAutoria(int pUsuarioID, List<int?>? pPublicIDS)
        {
            if (pPublicIDS == null || !pPublicIDS.Any()) return true;

            foreach (int publicID in pPublicIDS)
            {
                bool result = await _daoBDOfertas.VerificarAutoria(publicID, pUsuarioID);
                
                if (!result) return false; 
               
            }

            return true; 
        }

        public async Task<int> VerificarDescuento(int pPublicID)
        {
            int descuento = await _daoBDOfertas.VerificarDescuento(pPublicID);
            return descuento;
        }

        public async Task<OfertaSalidaC> CrearOferta(OfertaCreacion oferta)
        {
            int usuarioId = await _metodosDeValidacion.ObtenerUsuarioIDToken();
            oferta.Oferta_UsuarioID = usuarioId;
            oferta.Oferta_FCreacion = DateTime.Now;

            OfertaSalida ofertaCreada = await _daoBDOfertas.CrearOferta(oferta);

            if (ofertaCreada == null) return null;

            OfertaSalida ofertaSalida = await _daoBDOfertas.ObtenerOfertaPorID(ofertaCreada.Oferta_ID);

            OfertaSalidaC ofertaSalidaC = new OfertaSalidaC
            {   
                Oferta_FCreacion = DateTime.Now,
                Oferta_ID = ofertaSalida.Oferta_ID,
                Oferta_Nombre = ofertaSalida.Oferta_Nombre,
                Oferta_UsuarioID = ofertaSalida.Oferta_UsuarioID,
                Oferta_ProdOfer = ofertaSalida.Oferta_ProdOfer,
                Oferta_Descuento = ofertaSalida.Oferta_Descuento,
                Oferta_FInicio = ofertaSalida.Oferta_FInicio,
                Oferta_FFin = ofertaSalida.Oferta_FFin
            };

            return ofertaSalidaC;
        }

        public async Task<bool> EditarOferta(int pId, OfertaModif pOfertaModif)
        {
            DateTime fechaActual = DateTime.Now;
            pOfertaModif.Oferta_FModif = fechaActual;

            bool actualizado = await _daoBDOfertas.EditarOferta(pId, pOfertaModif);
 
            return actualizado;
        }

        public async Task<bool> DesasociarPublicaciones(int pId)
        {
            bool resultado = await _daoBDOfertas.DesasociarPublicaciones(pId);
            return resultado;
        }

        public async Task<bool> OfertaCancelar(int pUsuarioID, int pOfertaID)
        {
            DateTime fecha = DateTime.Now;
            return await _daoBDOfertas.OfertaCancelar(pUsuarioID, pOfertaID, fecha);
        }

        public async Task<bool> OfertasCancelar(int pUsuarioID)
        {
            DateTime fecha = DateTime.Now;
            return await _daoBDOfertas.OfertaCancelar(pUsuarioID, null, fecha);
        }
        public async Task<bool> EliminarOferta(int pOfertaID)
        {
            bool resultado = await _daoBDOfertas.EliminarOferta(pOfertaID,null);

            return resultado;
        }

        public async Task<bool> EliminarOfertas(int pUsuarioID)
        {
            bool resultado = await _daoBDOfertas.EliminarOferta(null, pUsuarioID);

            return resultado;
        }


    }
}
