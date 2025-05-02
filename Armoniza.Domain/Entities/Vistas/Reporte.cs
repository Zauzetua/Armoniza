using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Domain.Entities.Vistas
{
    public partial class Reporte
    {
        public int id { get; set; }
        public string usuario { get; set; } = null!;
        public string? grupo { get; set; }
        public string fecha_dado { get; set; } = null!;
        public string fecha_regreso { get; set; } = null!;
        public string retornado { get; set; } = null!;
        public string instrumento { get; set; } = null!;



    }
}
