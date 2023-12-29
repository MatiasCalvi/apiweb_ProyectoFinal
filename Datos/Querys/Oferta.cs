using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class OfertaQuerys : IOfertaQuerys
    {
        public string obtenerOfertaQuery { get; set; } = "SELECT o.Oferta_ID, o.Oferta_UsuarioID, o.Oferta_Nombre, o.Oferta_Descuento, o.Oferta_FInicio, o.Oferta_FFin, p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM ofertas o LEFT JOIN ofertas_publicaciones op ON o.Oferta_ID = op.OP_OfertaID LEFT JOIN publicaciones p ON op.OP_PublicID = p.Public_ID LEFT JOIN estados e ON p.Public_Estado = e.Estados_ID WHERE o.Oferta_ID = @Oferta_ID;";
        public string traerOfertasPorUsuarioID { get; set; } = "TraerOfertasPorUsuarioID";
        public string procesoAlmObt { get; set; } = "TraerOfertasActivas";
        public string procesoAlmCrear { get; set; } = "Creacion_Oferta";
        public string procesoAlmEdit { get; set; } = "Editar_Oferta";
        public string procesoAlmElim { get; set; } = "Eliminar_Oferta";
        public string procesoAlmVEstado { get; set; } = "VerificarEstado_Oferta";
        public string procesoAlmEstado { get; set; } = "CambiarEstado_Oferta";
        public string DesasociarPublicaciones { get; } = "DesasociarPublic_Ofertas";
        public string asociarProductoAOfertaQuery { get; set; } = "INSERT INTO ofertas_publicaciones(OP_OfertaID, OP_PublicID) VALUES (@Oferta_ID, @Public_ID);";
        public string publicacionesEnOfertaQuery { get; set; } = "SELECT COUNT(*) FROM ofertas_publicaciones WHERE OP_PublicID IN @ProductosIds";
        public string publicacionesUsuarioQuery { get; set; } = "SELECT COUNT(*) FROM publicaciones WHERE Public_ID IN @ProductosIds AND Public_UsuarioID = @UsuarioId";
        public string verificarCreador { get; set; } = "SELECT COUNT(*) FROM publicaciones p WHERE p.Public_UsuarioID = @Public_UsuarioID AND p.Public_ID = @Public_ID ";
    }
}