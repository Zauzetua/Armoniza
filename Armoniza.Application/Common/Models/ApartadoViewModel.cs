using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Models
{
    public class ApartadoViewModel
    {
        public apartado? apartado { get; set; }
        public IEnumerable<instrumento>? instrumentos{ get; set; }
        public List<int> instrumentosSeleccionados { get; set; } = new();
        public int MaxInstrumentos { get; set; } = 0;
    }
}
