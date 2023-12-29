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
        public string Public_Estado { get; set; }
        public PublicacionSalida() { }

        public PublicacionSalida(int public_ID, int public_UsuarioID, string public_Nombre, string public_Descripcion, decimal public_Precio, string public_Imagen, int public_Stock, string public_Estado)
        {
            Public_ID = public_ID;
            Public_UsuarioID = public_UsuarioID;
            Public_Nombre = public_Nombre;
            Public_Descripcion = public_Descripcion;
            Public_Precio = public_Precio;
            Public_Imagen = public_Imagen;
            Public_Stock = public_Stock;
            Public_Estado = public_Estado;
        }
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
