using Datos.Interfaces.IModelos;

namespace Datos.Modelos.DTO
{
    public class UsuarioSalida : IUsuario 
    {   
        public int Usuario_ID { get; set; }
        public string Usuario_Nombre { get; set; }
        public string Usuario_Apellido { get; set; }
        public string Usuario_Email { get; set; }
        public string Usuario_Role { get; set; }
        public string Usuario_Estado { get; set; }
        public UsuarioSalida() { }
    }
    public class UsuarioSalidaC : UsuarioSalida // *--> para crear el usuario
    {
        public DateTime Usuario_FCreacion { get; set; }
        public UsuarioSalidaC() { }
    }
    public class UsuarioSalidaU : UsuarioSalida // *--> usuario con la fecha de modificacion
    {
        public DateTime? Usuario_FModif { get; set; }
        public UsuarioSalidaU() { }
    }
}   

