namespace Datos.Interfaces.IQuerys
{
    public interface IAccesoQuerys
    {
        public string existeTokenQuery {  get; }
        public string actualizarTokenQuery { get; }
        public string crearTokenQuery { get; }
        public string eliminarTokenQuery { get; }
    }
}
