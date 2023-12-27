using Datos.Modelos.DTO;

public class OfertaRespuesta
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public OfertaSalida Oferta { get; set; }
}

