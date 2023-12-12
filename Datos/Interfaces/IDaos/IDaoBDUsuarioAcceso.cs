using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDUsuarioAcceso
    {
        Task<string> ObtenerRefreshToken(int pUsuarioId);
        Task GuardarRefreshToken(int usuarioId, string refreshToken);
        Task EliminarRefreshToken(int pUsuarioId);
    }
}
