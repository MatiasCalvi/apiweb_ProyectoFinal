using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Interfaces.IModelos
{
    public interface IPublicacion
    {
        public string Public_Nombre { get; set; }
        public string Public_Descripcion { get; set; }
        public decimal Public_Precio { get; set; }
        public string Public_Imagen { get; set; }
        public int Public_Stock { get; set; }
    }
}
