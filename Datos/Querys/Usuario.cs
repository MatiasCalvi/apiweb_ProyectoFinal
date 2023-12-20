using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class UsuariosQuery : IUsuarioQuerys
    {
        public string obtenerUsuarioIDQuery { get; set; } = "SELECT * FROM usuarios WHERE Usuario_ID = @Usuario_ID";
        public string obtenerUsuarioEmailQuery { get; set; } = "SELECT * FROM usuarios WHERE Usuario_Email = @Usuario_Email";
        public string esAdministradorQuery { get; set; } = "SELECT COUNT(*) FROM email_admins WHERE EA_Email = @EA_Email";
        public string crearUsuarioQuery { get; set; } = "INSERT INTO usuarios(Usuario_Nombre, Usuario_Apellido, Usuario_Email, Usuario_Contra, Usuario_FCreacion) VALUES(@Usuario_Nombre, @Usuario_Apellido, @Usuario_Email, @Usuario_Contra, @Usuario_FCreacion); SELECT* FROM usuarios WHERE Usuario_ID = LAST_INSERT_ID()";
    }
}
