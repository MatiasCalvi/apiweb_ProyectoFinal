namespace Datos.Interfaces.IQuerys
{
    public interface IOfertaQuerys
    {
        public string obtenerOfertasQuery { get; }
        public string obtenerOfertaQuery { get;}
        public string obtenerfertasPorUsuarioIDQuery { get; }
        public string asociarProductoAOfertaQuery { get; }
        public string publicacionesEnOfertaQuery { get;}
        public string publicacionesUsuarioQuery { get;}
        public string verificarCreador { get; }
        public string procesoAlmCrear { get; }
        public string procesoAlmEdit { get; }
        public string procesoAlmElim { get; }
    }
}
