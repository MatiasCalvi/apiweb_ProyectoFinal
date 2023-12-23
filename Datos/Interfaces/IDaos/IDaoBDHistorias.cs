using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDHistorias 
    {
        Task<List<HistoriaCompraSalida>> ObtenerHistorial(int pUsuarioID);
    }
}
