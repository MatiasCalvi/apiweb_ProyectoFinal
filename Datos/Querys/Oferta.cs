using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class OfertaQuerys : IOfertaQuerys
    {
        public string obtenerOferta { get; set; } = "SELECT o.Oferta_ID,o.Oferta_UsuarioID, o.Oferta_Nombre, o.Oferta_Descuento, o.Oferta_FInicio, o.Oferta_FFin, op.OP_PublicID AS Public_ID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock, e.Estados_Nombre AS Public_Estado FROM ofertas o JOIN ofertas_publicaciones op ON o.Oferta_ID = op.OP_OfertaID JOIN publicaciones p ON op.OP_PublicID = p.Public_ID JOIN estados e ON p.Public_Estado = e.Estados_ID WHERE o.Oferta_ID = @Oferta_ID;";
                                                        
    }
}
