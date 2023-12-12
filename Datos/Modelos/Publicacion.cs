using Datos.Interfaces.IModelos;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Datos.Modelos
{
    public class PublicacionCreacion : IPublicacion
    {
        [JsonIgnore]
        public int Public_UsuarioID {  get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Public_Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Public_Descripcion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal Public_Precio { get; set; }
        
        [Url(ErrorMessage = "La URL de la imagen no es válida.")]
        public string Public_Imagen { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor que cero")]
        public int Public_Stock { get; set; }

        [JsonIgnore]
        public DateTime Public_FCreacion { get; set;}
    }

    public class PublicacionModif 
    {
        [JsonIgnore]
        public int? Public_UsuarioID { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string? Public_Nombre { get; set; }

        public string? Public_Descripcion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal? Public_Precio { get; set; }

        [Url(ErrorMessage = "La URL de la imagen no es válida.")]
        public string? Public_Imagen { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser igual o mayor que cero.")]
        public int? Public_Stock { get; set; }

        [JsonIgnore]
        public DateTime? Public_FModif { get; set; }
    }

    public class PublicacionModifA 
    {
        public int? Public_UsuarioID { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string? Public_Nombre { get; set; }

        public string? Public_Descripcion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
        public decimal? Public_Precio { get; set; }

        [Url(ErrorMessage = "La URL de la imagen no es válida.")]
        public string? Public_Imagen { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser igual o mayor que cero.")]
        public int? Public_Stock { get; set; }

        [JsonIgnore]
        public DateTime? Public_FModif { get; set; }
    }
}
