using Datos.Interfaces.IModelos;
using Datos.Modelos.DTO;
using System.Text.Json.Serialization;

namespace Datos.Modelos
{
    public class OfertaCreacion : IOferta
    {
        [JsonIgnore]
        public int Oferta_ID { get; set; }
        public int Oferta_UsuarioID { get; set; }
        public string Oferta_Nombre { get; set; }
        public int Oferta_Descuento { get; set; }
        public DateTime Oferta_FInicio { get; set; }
        public DateTime Oferta_FFin { get; set; }
    }

}
