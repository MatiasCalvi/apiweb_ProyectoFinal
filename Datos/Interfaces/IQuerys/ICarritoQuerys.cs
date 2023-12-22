namespace Datos.Interfaces.IQuerys
{
    public interface ICarritoQuerys
    {
        public string obtenerCarritoQuery { get;}
        public string agregarProducto { get; }
        public string verificarDuplicado { get;}
        public string agregarAlHistorialQuery { get;}
        public string eliminarQuery { get; }
        public string eliminarTodoQuery { get; }
    }
}
