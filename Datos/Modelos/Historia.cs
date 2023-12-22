using Datos.Interfaces.IModelos;
using System.Text.Json.Serialization;

namespace Datos.Modelos
{
    public class Historia : IHistoriaCompra
    {
        [JsonIgnore]
        public int HC_ID { get; set; }
        public int HC_UsuarioID { get; set; }
        public int HC_PID { get; set; }
        public int HC_Unidades { get; set; }
        public decimal HC_PrecioFinal { get; set; }
        public DateTime HC_FechaCompra { get; set; }
    }
}
