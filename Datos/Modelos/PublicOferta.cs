using System.Text.Json.Serialization;

namespace Datos.Modelos
{
    public class PublicOfertaCreacion
    {
        [JsonIgnore]
        public int OP_ID {  get; set; }
        public int OP_OfertaID { get; set; }
        public int OP_PublicID { get; set; }
    }
}
