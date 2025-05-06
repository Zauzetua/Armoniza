using Armoniza.Domain.Entities.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Models
{
    public class ReporteViewModel
    {
        public List<Reporte> Reportes { get; set; } = new();

        // Ordenamiento
        public string OrdenarPor { get; set; } = "fecha_dado";
        public string Direccion { get; set; } = "asc";

        // Filtros
        public string? FiltroUsuario { get; set; }
        public string? FiltroGrupo { get; set; }
        public string? FiltroRetornado { get; set; }
        public string? FiltroInstrumento { get; set; }

        // Paginación
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public string? FechaDesde { get; set; }
        public string? FechaHasta { get; set; }
        

    }


}
