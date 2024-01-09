namespace Datos.Querys
{
    public class HistoriaQuerys 
    {
        public static string obtenerHistorialQuery { get; set; } = "SELECT * FROM historial_compras WHERE HC_UsuarioID = @HC_UsuarioID";

    }
}
