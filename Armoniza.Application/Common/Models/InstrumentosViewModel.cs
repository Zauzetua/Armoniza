using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Domain.Entities;

namespace Armoniza.Application.Common.Models
{
    public class InstrumentosViewModel
    {
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public List<instrumento> Instrumentos { get; set; } = new();
    }
}
