﻿namespace Datos.Interfaces.IQuerys
{
    public interface IAdminQuerys
    {
        public string obtenerUsuariosQuery { get; }
        public string obtenerPublicacionesQuery { get; }
        public string obtenerPublicacionesDeUnUsuarioQuery { get; }
        public string obtenerCarritosQuery { get; }
        public string obtenerHistorialesQuery { get; }
        public string procesoAlmObt { get; }
        public string verificarUsuarioDeshabilitadoQuery { get; }
        public string verificarUsuarioHabilitadoQuery { get; }
        public string habilitarUsuarioQuery { get; }
        public string desactivarUsuarioQuery { get; }
        public string asignarRolAAdminQuery { get; }
        public string asignarRolAUsuarioQuery { get; }
        public string procesoAlmEditAdmin { get; }
        public string procesoAlmCan { get; }
    }
}
