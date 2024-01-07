using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class AdminQuerys : IAdminQuerys
    {
        public string obtenerUsuariosQuery { get; set; } = "SELECT u.Usuario_ID, u.Usuario_Nombre, u.Usuario_Apellido, u.Usuario_Email, r.Rol_Nombre AS Usuario_Role, e.Estados_Nombre AS Usuario_Estado FROM usuarios u JOIN estados e ON u.Usuario_Estado = e.Estados_Id JOIN roles r ON u.Usuario_Role = r.Rol_ID";
        public string obtenerPublicacionesQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id";
        public string obtenerPublicacionesDeUnUsuarioQuery { get; set; } = "SELECT p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM publicaciones p JOIN estados e ON p.Public_Estado = e.Estados_Id WHERE Public_UsuarioID = @Public_UsuarioID";
        public string obtenerCarritosQuery { get; set; } = "SELECT c.Carrito_UsuarioID, p.Public_ID as Carrito_PID, c.Carrito_ProdUnidades, p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM carrito c JOIN publicaciones p ON c.Carrito_PID = p.Public_ID JOIN estados e ON p.Public_Estado = e.Estados_Id ORDER BY c.Carrito_UsuarioID;";
        public string obtenerHistorialesQuery { get; set; } = "SELECT * FROM historial_compras ORDER BY(HC_UsuarioID)";
        public string procesoAlmObt { get; set; } = "TraerTodasLOfertas";
        public string verificarUsuarioDeshabilitadoQuery { get; set; } = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 2";
        public string verificarUsuarioHabilitadoQuery { get; set; } = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 1";
        public string habilitarUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Estado = 1 WHERE Usuario_ID = @Usuario_ID";
        public string desactivarUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Estado = 2 WHERE Usuario_ID = @Usuario_ID";
        public string asignarRolAAdminQuery { get; set; } = "UPDATE usuarios SET Usuario_Role = 2 WHERE Usuario_ID = @Usuario_ID";
        public string asignarRolAUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Role = 1 WHERE Usuario_ID = @Usuario_ID";
        public string procesoAlmEdit { get; set; } = "EditarAdmin_Oferta";
        public string procesoAlmCan { get; set; } = "Cancelar_Ofertas";
    }
}
