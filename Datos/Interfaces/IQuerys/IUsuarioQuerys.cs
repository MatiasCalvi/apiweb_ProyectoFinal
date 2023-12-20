namespace Datos.Interfaces.IQuerys
{
    public interface IUsuarioQuerys
    {
        public string obtenerUsuarioIDQuery {  get; }
        public string obtenerUsuarioEmailQuery { get; }
        public string esAdministradorQuery { get; }
        public string crearUsuarioQuery { get; }
    }
}
