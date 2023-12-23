using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class HistoriaQuerys : IHistoriaQuerys
    {
        public string obtenerHistorialQuery { get; set; } = "SELECT * FROM historial_compras WHERE HC_UsuarioID = @HC_UsuarioID";

    }
}
