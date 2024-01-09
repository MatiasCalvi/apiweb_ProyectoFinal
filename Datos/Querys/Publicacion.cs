namespace Datos.Querys
{
    public class PublicacionQuerys 
    {
        public static string obtenerPublicacionesQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_Estado = '3'";
        public static string obtenerPublicacionIDQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_ID = @Public_ID";
        public static string obtenerPublicacionesDeUnUsuarioQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_UsuarioID = @Public_UsuarioID";
        public static string obtenerStockPublicacionQuery { get; set; } = "SELECT * FROM publicaciones WHERE Public_ID = @Public_ID";
        public static string proceAlmBuscar{ get; set; } = "Publicaciones_Buscar";
        public static string procesoAlmVEstado { get; set; } = "VerificarEstado_Public"; 
        public static string procesoAlmEstado { get; set; } = "CambiarEstado_Public";
        public static string procesoAlmElim { get; set; } = "Eliminar_Public";
        public static string crearPublicacionQuery { get; set; } = "INSERT INTO publicaciones(Public_UsuarioID, Public_Nombre, Public_Descripcion, Public_Precio, Public_Imagen, Public_Stock,Public_FCreacion) VALUES (@Public_UsuarioID, @Public_Nombre, @Public_Descripcion, @Public_Precio, @Public_Imagen, @Public_Stock, @Public_FCreacion);SELECT * FROM publicaciones WHERE Public_ID = LAST_INSERT_ID();";
        public static string ActivarPublicQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 3 WHERE Public_ID = @Public_ID AND Public_UsuarioID = @Public_UsuarioID";
        public static string PausarPublicQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 4 WHERE Public_ID = @Public_ID AND Public_UsuarioID = @Public_UsuarioID";
        public static string CancelarPublicQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 5 WHERE Public_ID = @Public_ID AND Public_UsuarioID = @Public_UsuarioID";
        public static string VerificarPublicPausadaQuery { get; set; } = "SELECT 1 FROM publicaciones WHERE Public_ID = @Public_ID AND Public_Estado = 4";
        public static string VerificarPublicCanceladaQuery { get; set; } = "SELECT 1 FROM publicaciones WHERE Public_ID = @Public_ID AND Public_Estado = 5";
        public static string VerificarPublicActivadaQuery { get; set; } = "SELECT 1 FROM publicaciones WHERE Public_ID = @Public_ID AND Public_Estado = 3";
        public static string PausarPublicAdminQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 4 WHERE Public_ID = @Public_ID";
        public static string CancelarPublicAdminQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 5 WHERE Public_ID = @Public_ID";
        public static string ActivarPublicAdminQuery { get; set; } = "UPDATE publicaciones SET Public_Estado = 3 WHERE Public_ID = @Public_ID";
    }
}
