using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class AccesoQuerys : IAccesoQuerys
    {
        public string existeTokenQuery { get; set; } = "SELECT RefreshTU_Token FROM refreshtokenu WHERE refreshTU_UsuaID = @Usuario_ID";
        public string actualizarTokenQuery { get; set; } = "UPDATE refreshtokenu SET RefreshTU_Token = @RefreshToken WHERE refreshTU_UsuaID = @Usuario_ID";
        public string crearTokenQuery { get; set; } = "INSERT INTO refreshtokenu (RefreshTU_UsuaID, RefreshTU_Token) VALUES (@Usuario_ID, @RefreshToken)";
        public string eliminarTokenQuery { get; set; } = "DELETE FROM refreshtokenu WHERE RefreshTU_UsuaID = @Usuario_ID";
    }
}
