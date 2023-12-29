namespace Datos.Interfaces.IQuerys
{
    public interface IPublicacionQuerys
    {
        public string obtenerPublicacionesQuery { get; }
        public string obtenerPublicacionIDQuery { get; }
        public string obtenerPublicacionesDeUnUsuarioQuery { get; }
        public string obtenerStockPublicacionQuery { get; }
        public string proceAlmBuscar { get;}
        public string procesoAlmElim { get;}
        public string crearPublicacionQuery { get; }
        public string procesoAlmVEstado { get; }
        public string procesoAlmEstado { get; }
    }
}
