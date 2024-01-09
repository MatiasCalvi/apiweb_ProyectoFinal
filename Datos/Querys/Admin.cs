namespace Datos.Querys
{
    public class AdminQuerys 
    {
        public static string obtenerUsuariosQuery = "SELECT u.Usuario_ID, u.Usuario_Nombre, u.Usuario_Apellido, u.Usuario_Email, r.Rol_Nombre AS Usuario_Role, e.Estados_Nombre AS Usuario_Estado FROM usuarios u JOIN estados e ON u.Usuario_Estado = e.Estados_Id JOIN roles r ON u.Usuario_Role = r.Rol_ID";
        public static string obtenerPublicacionesQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id";
        public static string obtenerPublicacionesDeUnUsuarioQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_UsuarioID = @Public_UsuarioID";
        public static string obtenerCarritosQuery { get; set; } = "SELECT c.Carrito_UsuarioID, p.Public_ID as Carrito_PID, c.Carrito_ProdUnidades, p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM carrito c JOIN publicaciones p ON c.Carrito_PID = p.Public_ID JOIN estados e ON p.Public_Estado = e.Estados_Id ORDER BY c.Carrito_UsuarioID;";
        public static string obtenerHistorialesQuery { get; set; } = "SELECT * FROM historial_compras ORDER BY(HC_UsuarioID)"; 
        public static string obtenerHistorialQuery { get; set; } = "SELECT * FROM historial_compras WHERE HC_UsuarioID = @HC_UsuarioID"; 
        public static string procesoAlmObt { get; set; } = "TraerTodasLOfertas";
        public static string desasociarPublicacionesAdmin { get; set; } = "DesasociarPublicOfertas_Admin";
        public static string verificarUsuarioDeshabilitadoQuery { get; set; } = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 2";
        public static string verificarUsuarioHabilitadoQuery { get; set; } = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 1";
        public static string habilitarUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Estado = 1 WHERE Usuario_ID = @Usuario_ID";
        public static string desactivarUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Estado = 2 WHERE Usuario_ID = @Usuario_ID";
        public static string asignarRolAAdminQuery { get; set; } = "UPDATE usuarios SET Usuario_Role = 2 WHERE Usuario_ID = @Usuario_ID";
        public static string asignarRolAUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Role = 1 WHERE Usuario_ID = @Usuario_ID";
        public static string procesoAlmEditAdmin { get; set; } = "Editar_OfertaAdmin";
        public static string procesoAlmCan { get; set; } = "Cancelar_Ofertas";
    }
}
