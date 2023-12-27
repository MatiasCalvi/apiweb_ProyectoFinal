using Datos.Interfaces.IModelos;

namespace Datos.Modelos.DTO
{
    public class OfertaSalida : IOferta
    {
        public int Oferta_ID { get; set; }
        public int Oferta_UsuarioID { get; set; }
        public string Oferta_Nombre { get; set; }
        public List<PublicacionSalida> Oferta_ProdOfer { get; set; }
        public int Oferta_Descuento { get; set; }
        public DateTime Oferta_FInicio { get; set; }
        public DateTime Oferta_FFin { get; set; }
    }
    public class OfertaSalidaC : OfertaSalida
    {
        public DateTime Oferta_FCreacion { get; set; }
    }
}
