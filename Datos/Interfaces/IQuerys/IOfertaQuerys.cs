namespace Datos.Interfaces.IQuerys
{
    public interface IOfertaQuerys
    {
        public string obtenerOfertaQuery { get;}
        public string traerOfertasPorUsuarioID { get; }
        public string asociarProductoAOfertaQuery { get; }
        public string publicacionesEnOfertaQuery { get;}
        public string publicacionesUsuarioQuery { get;}
        public string verificarCreador { get; }
        public string procesoAlmObt { get;}
        public string procesoAlmCrear { get; }
        public string procesoAlmEdit { get; }
        public string procesoAlmElim { get; }
        public string procesoAlmVEstado { get; }
        public string procesoAlmEstado { get; }
        public string DesasociarPublicaciones { get; }

    }
}
