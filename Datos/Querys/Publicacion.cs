using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class PublicacionQuerys : IPublicacionQuerys
    {
        public string obtenerPublicacionesQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_Estado = '3'";
        public string obtenerPublicacionIDQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_ID = @Public_ID";
        public string obtenerPublicacionesDeUnUsuarioQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_UsuarioID = @Public_UsuarioID";
        public string obtenerStockPublicacionQuery { get; set; } = "SELECT * FROM publicaciones WHERE Public_ID = @Public_ID";
        public string proceAlmBuscar{ get; set; } = "Publicaciones_Buscar";
        public string procesoAlmVEstado { get; set; } = "VerificarEstado_Public"; 
        public string procesoAlmEstado { get; set; } = "CambiarEstado_Public";
        public string procesoAlmElim { get; set; } = "Eliminar_Public";
        public string crearPublicacionQuery { get; set; } = "INSERT INTO publicaciones(Public_UsuarioID, Public_Nombre, Public_Descripcion, Public_Precio, Public_Imagen, Public_Stock,Public_FCreacion) VALUES (@Public_UsuarioID, @Public_Nombre, @Public_Descripcion, @Public_Precio, @Public_Imagen, @Public_Stock, @Public_FCreacion);SELECT * FROM publicaciones WHERE Public_ID = LAST_INSERT_ID();";
        public string ActivarPublicQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 3 WHERE Public_ID = @Public_ID AND Public_UsuarioID = @Public_UsuarioID";
        public string PausarPublicQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 4 WHERE Public_ID = @Public_ID AND Public_UsuarioID = @Public_UsuarioID";
        public string CancelarPublicQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 5 WHERE Public_ID = @Public_ID AND Public_UsuarioID = @Public_UsuarioID";
        public string VerificarPublicPausadaQuery { get; set; } = "SELECT 1 FROM publicaciones WHERE Public_ID = @Public_ID AND Public_Estado = 4";
        public string VerificarPublicCanceladaQuery { get; set; } = "SELECT 1 FROM publicaciones WHERE Public_ID = @Public_ID AND Public_Estado = 5";
        public string VerificarPublicActivadaQuery { get; set; } = "SELECT 1 FROM publicaciones WHERE Public_ID = @Public_ID AND Public_Estado = 3";
        public string PausarPublicAdminQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 4 WHERE Public_ID = @Public_ID";
        public string CancelarPublicAdminQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 5 WHERE Public_ID = @Public_ID";
        public string ActivarPublicAdminQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 3 WHERE Public_ID = @Public_ID";
    }
}
