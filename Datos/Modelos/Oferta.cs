using Datos.Interfaces.IModelos;
using Datos.Modelos.DTO;
using System.Text.Json.Serialization;

namespace Datos.Modelos
{
    public class OfertaCreacion : IOferta
    {
        [JsonIgnore]
        public int Oferta_ID { get; set; }

        [JsonIgnore]
        public int Oferta_UsuarioID { get; set; }
        public string Oferta_Nombre { get; set; }
        public List<int> Oferta_ProdOfer { get; set; }
        public int Oferta_Descuento { get; set; }
        public DateTime Oferta_FInicio { get; set; }
        public DateTime Oferta_FFin { get; set; }

        [JsonIgnore]
        public DateTime Oferta_FCreacion { get; set; }
    }
    public class OfertaModif
    {
        [JsonIgnore]
        public int? Oferta_ID { get; set; }

        [JsonIgnore]
        public int? Oferta_UsuarioID { get; set; }
        public string? Oferta_Nombre { get; set; }
        public List<int?>? Oferta_ProdOfer { get; set; }
        public int? Oferta_Descuento { get; set; }
        public DateTime? Oferta_FInicio { get; set; }
        public DateTime? Oferta_FFin { get; set; }

        [JsonIgnore]
        public DateTime? Oferta_FModif { get; set; }
    }

    public class OfertaModifA
    {
        [JsonIgnore]
        public int? Oferta_ID { get; set; }
        public int? Oferta_UsuarioID { get; set; }
        public string? Oferta_Nombre { get; set; }
        public List<int?>? Oferta_ProdOfer { get; set; }
        public int? Oferta_Descuento { get; set; }
        public DateTime? Oferta_FInicio { get; set; }
        public DateTime? Oferta_FFin { get; set; }

        [JsonIgnore]
        public DateTime? Oferta_FModif { get; set; }
    }
}
