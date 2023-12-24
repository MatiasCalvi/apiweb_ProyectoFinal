using Datos.Interfaces.IModelos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Datos.Modelos
{
    public class UsuarioCreacion : IUsuario
    {   
        [Required(ErrorMessage = "El Nombre es requerido.", AllowEmptyStrings = false)]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "El nombre debe comenzar con letra mayúscula y no puede contener números, guiones ni guiones bajos.")]
        public string Usuario_Nombre { get; set; }

        [Required(ErrorMessage = "El Apellido es requerido.", AllowEmptyStrings = false)]
        [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "El Apellido debe comenzar con letra mayúscula y no puede contener números, guiones ni guiones bajos.")]
        public string Usuario_Apellido { get; set; }

        [Required(ErrorMessage = "El Email es requerido.", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Usuario_Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerido.", AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(20, ErrorMessage = "La contraseña no puede tener más de 20 caracteres.")]
        [RegularExpression("^(?=.*[A-Z])[A-Za-z0-9]{6,20}$", ErrorMessage = "La contraseña debe tener al menos una letra mayúscula y no puede contener tildes ni espacios en blanco.")]
        public string Usuario_Contra { get; set; }

        [AllowNull]
        [JsonIgnore]
        public int? Usuario_Role { get; set; }
        
        [AllowNull]
        [JsonIgnore]
        public int Usuario_Estado { get; set; }

        [AllowNull]
        [JsonIgnore]
        public DateTime? Usuario_FCreacion { get; set; }
    }
    public class UsuarioModif : IUsuario
    {
        [AllowNull]
        [JsonIgnore]
        public int Usuario_ID { get; set; }

        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "El nombre debe comenzar con letra mayúscula y no puede contener números, guiones ni guiones bajos.")]
        public string? Usuario_Nombre { get; set; }

        [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres.")]
        [MaxLength(50, ErrorMessage = "El apellido no puede tener más de 50 caracteres.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "El Apellido debe comenzar con letra mayúscula y no puede contener números, guiones ni guiones bajos.")]
        public string? Usuario_Apellido { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Usuario_Email { get; set; }

        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [MaxLength(20, ErrorMessage = "La contraseña no puede tener más de 20 caracteres.")]
        [RegularExpression("^(?=.*[A-Z])[A-Za-z0-9]{6,20}$", ErrorMessage = "La contraseña debe tener al menos una letra mayúscula y no puede contener tildes ni espacios en blanco.")]
        public string? Usuario_Contra { get; set; }
        
        [AllowNull]
        [JsonIgnore]
        public string? Usuario_Role { get; set; }

        [AllowNull]
        [JsonIgnore]
        public DateTime? Usuario_FModif { get; set; }
    }
    public class UsuarioLogin
    {
        public string Usuario_Email { get; set; }
        public string Usuario_Contra {  get; set; }
    }

}
