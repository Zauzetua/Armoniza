using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Domain.Entities.Vistas;

namespace Armoniza.Application.Common.Interfaces.Services
{
    public interface IReportesService
    {
        Task<List<Reporte>> ObtenerRegistros();
        Task<byte[]> GenerarExcel(
         string ordenarPor,
         string direccion,
         string? filtroUsuario,
         string? filtroGrupo,
         string? filtroRetornado,
         string? filtroInstrumento,
         string? fechaDesde,
          string? fechaHasta);
    }
}
