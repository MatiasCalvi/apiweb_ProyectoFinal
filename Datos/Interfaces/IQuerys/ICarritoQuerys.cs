namespace Datos.Interfaces.IQuerys
{
    public interface ICarritoQuerys
    {
        public string obtenerCarritoQuery { get;}
        public string agregarProducto { get; }
        public string verificarDuplicado { get;}
        public string comprarQuery { get;}
    }
}
