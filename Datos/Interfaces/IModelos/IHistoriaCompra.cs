﻿namespace Datos.Interfaces.IModelos
{
    public interface IHistoriaCompra
    {
        public int HC_ID { get; set; }
        public int HC_UsuarioID { get; set; }
        public int HC_PID { get; set; }
        public int HC_Unidades { get; set; }
        public decimal HC_PrecioFinal { get; set; }
        public DateTime HC_FechaCompra { get; set; }

    }
}
