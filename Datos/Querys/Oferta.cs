namespace Datos.Querys
{
    public class OfertaQuerys 
    {
        public static string obtenerOfertaQuery { get; set; } = "SELECT o.Oferta_ID, o.Oferta_UsuarioID, o.Oferta_Nombre, o.Oferta_Descuento, o.Oferta_FInicio, o.Oferta_FFin, p.Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM ofertas o LEFT JOIN ofertas_publicaciones op ON o.Oferta_ID = op.OP_OfertaID LEFT JOIN publicaciones p ON op.OP_PublicID = p.Public_ID LEFT JOIN estados e ON p.Public_Estado = e.Estados_ID WHERE o.Oferta_ID = @Oferta_ID;";
        public static string traerOfertasPorUsuarioID { get; set; } = "TraerOfertasPorUsuarioID";
        public static string procesoAlmObt { get; set; } = "TraerOfertasActivas";
        public static string procesoAlmCrear { get; set; } = "Creacion_Oferta";
        public static string procesoAlmEdit { get; set; } = "Editar_Oferta"; 
        public static string procesoAlmElim { get; set; } = "Eliminar_Oferta";
        public static string procesoAlmCan { get; set; } = "Cancelar_Ofertas";
        public static string procesoAlmVEstado { get; set; } = "VerificarEstado_Oferta";
        public static string procesoAlmEstado { get; set; } = "CambiarEstado_Oferta";
        public static string procesoVDescuento { get; set; } = "Verificar_Descuento";
        public static string desasociarPublicaciones { get; } = "DesasociarPublic_Ofertas"; 
        public static string asociarProductoAOfertaQuery { get; set; } = "INSERT INTO ofertas_publicaciones(OP_OfertaID, OP_PublicID) VALUES (@Oferta_ID, @Public_ID);";
        public static string publicacionesEnOfertaQuery { get; set; } = "SELECT COUNT(*) FROM ofertas_publicaciones WHERE OP_PublicID IN @ProductosIds";
        public static string publicacionesUsuarioQuery { get; set; } = "SELECT COUNT(*) FROM publicaciones WHERE Public_ID IN @ProductosIds AND Public_UsuarioID = @UsuarioId";
        public static string verificarCreador { get; set; } = "SELECT COUNT(*) FROM publicaciones p WHERE p.Public_UsuarioID = @Public_UsuarioID AND p.Public_ID = @Public_ID ";
    }
}