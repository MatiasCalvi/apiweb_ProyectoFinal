using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class UsuariosQuery : IUsuarioQuerys
    {
        public string obtenerUsuarioIDQuery { get; set; } ="SELECT Usuario_ID, Usuario_Nombre, Usuario_Apellido, Usuario_Email, r.Rol_Nombre AS Usuario_Role, e.Estados_Nombre AS Usuario_Estado FROM usuarios u JOIN estados e ON u.Usuario_Estado = e.Estados_Id JOIN roles r ON u.Usuario_Role = r.Rol_ID WHERE u.Usuario_ID = @Usuario_ID";
        public string obtenerUsuarioEmailQuery { get; set; } = "SELECT Usuario_ID, Usuario_Nombre, Usuario_Apellido, Usuario_Email, Usuario_Contra, r.Rol_Nombre AS Usuario_Role, e.Estados_Nombre AS Usuario_Estado FROM usuarios u JOIN estados e ON u.Usuario_Estado = e.Estados_Id JOIN roles r ON u.Usuario_Role = r.Rol_ID WHERE u.Usuario_Email = @Usuario_Email";
        public string crearUsuarioQuery { get; set; } = "INSERT INTO usuarios(Usuario_Nombre, Usuario_Apellido, Usuario_Email, Usuario_Contra, Usuario_FCreacion) VALUES(@Usuario_Nombre, @Usuario_Apellido, @Usuario_Email, @Usuario_Contra, @Usuario_FCreacion); SELECT* FROM usuarios WHERE Usuario_ID = LAST_INSERT_ID()";
    }
}
