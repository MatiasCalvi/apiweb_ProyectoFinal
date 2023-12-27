using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class OfertaQuerys : IOfertaQuerys
    {
        public string obtenerOfertasQuery { get; set; } = "SELECT o.Oferta_ID, o.Oferta_UsuarioID, o.Oferta_Nombre, o.Oferta_Descuento, o.Oferta_FInicio, o.Oferta_FFin, op.OP_PublicID AS Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM ofertas o JOIN ofertas_publicaciones op ON o.Oferta_ID = op.OP_OfertaID JOIN publicaciones p ON op.OP_PublicID = p.Public_ID JOIN estados e ON p.Public_Estado = e.Estados_ID WHERE o.Oferta_Estado = @Oferta_Estado;";
        public string obtenerOfertaQuery { get; set; } = "SELECT o.Oferta_ID, o.Oferta_UsuarioID, o.Oferta_Nombre, o.Oferta_Descuento, o.Oferta_FInicio, o.Oferta_FFin, op.OP_PublicID AS Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM ofertas o JOIN ofertas_publicaciones op ON o.Oferta_ID = op.OP_OfertaID JOIN publicaciones p ON op.OP_PublicID = p.Public_ID JOIN estados e ON p.Public_Estado = e.Estados_ID WHERE o.Oferta_ID = @Oferta_ID;";
        public string obtenerfertasPorUsuarioIDQuery { get; set; } = "SELECT o.Oferta_ID, o.Oferta_UsuarioID, o.Oferta_Nombre, o.Oferta_Descuento, o.Oferta_FInicio, o.Oferta_FFin, op.OP_PublicID AS Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM ofertas o JOIN ofertas_publicaciones op ON o.Oferta_ID = op.OP_OfertaID JOIN publicaciones p ON op.OP_PublicID = p.Public_ID JOIN estados e ON p.Public_Estado = e.Estados_ID WHERE o.Oferta_UsuarioID = @Oferta_UsuarioID;";
        public string procesoAlmCrear { get; set; } = "Creacion_Oferta";
        public string procesoAlmEdit { get; set; } = "Editar_Oferta";
        public string asociarProductoAOfertaQuery { get; set; } = "INSERT INTO ofertas_publicaciones(OP_OfertaID, OP_PublicID) VALUES (@Oferta_ID, @Public_ID);";
        public string publicacionesEnOfertaQuery { get; set; } = "SELECT COUNT(*) FROM ofertas_publicaciones WHERE OP_PublicID IN @ProductosIds";
        public string publicacionesUsuarioQuery { get; set; } = "SELECT COUNT(*) FROM publicaciones WHERE Public_ID IN @ProductosIds AND Public_UsuarioID = @UsuarioId";
        public string verificarCreador { get; set; } = "SELECT COUNT(*) FROM publicaciones p WHERE p.Public_UsuarioID = @Public_UsuarioID AND p.Public_ID = @Public_ID ";
    }
}
