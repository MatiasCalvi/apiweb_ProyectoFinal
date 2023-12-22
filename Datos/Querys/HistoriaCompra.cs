using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class HistoriaQuerys : IHistoriaQuerys
    {
        public int HC_ID { get; set; }
        public int HC_UsuarioID { get; set; }
        public int HC_PID { get; set; }
        public int HC_Unidades { get; set; }
        public int HC_PrecioFinal { get; set; }
        public int HC_FechaCompra { get; set; }

        public HistoriaQuerys() { }
    }
}
