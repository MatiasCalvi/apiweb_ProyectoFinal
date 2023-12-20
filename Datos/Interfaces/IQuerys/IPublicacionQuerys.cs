namespace Datos.Interfaces.IQuerys
{
    public interface IPublicacionQuerys
    {
        public string obtenerPublicacionesQuery { get; }
        public string obtenerPublicacionIDQuery { get; }
        public string obtenerPublicacionesDeUnUsuarioQuery { get; }
        public string crearPublicacionQuery { get; }
        public string ActivarPublicQuery { get; }
        public string PausarPublicQuery { get; }
        public string CancelarPublicQuery { get; }
        public string VerificarPublicPausadaQuery { get; }
        public string VerificarPublicCanceladaQuery { get; }
        public string VerificarPublicActivadaQuery { get; }
        public string PausarPublicAdminQuery { get; }
        public string CancelarPublicAdminQuery { get; }
        public string ActivarPublicAdminQuery { get; }
    }
}
