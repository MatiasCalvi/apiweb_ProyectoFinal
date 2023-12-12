using Datos.Interfaces.IModelos;

namespace Datos.Modelos.DTO
{
    public class PublicacionSalida : IPublicacion
    {   
        public int Public_ID { get; set; }
        public int Public_UsuarioID { get; set; }
        public string Public_Nombre { get; set; }
        public string Public_Descripcion { get; set; }
        public decimal Public_Precio { get; set; }
        public string Public_Imagen { get; set; }
        public int Public_Stock { get; set; }
        public PublicacionSalida() { }
    }
    public class PublicacionSalidaC : PublicacionSalida
    {
        public DateTime Public_FCreacion {  get; set; }
        public PublicacionSalidaC() { }
    }
    public class PublicacionSalidaM : PublicacionSalida
    {
        public DateTime Public_FModif { get; set; }
        public PublicacionSalidaM() { }
    }
}
