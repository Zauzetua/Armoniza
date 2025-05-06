using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Domain.Entities.Vistas;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Globalization;

namespace Armoniza.Infrastructure.Services
{
    public class ReportesService : IReportesService
    {
        private readonly IReportesRepository _reportesRepository;
        public ReportesService(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<List<Reporte>> ObtenerRegistros()
        {
            var reportes = await _reportesRepository.ObtenerRegistros();

            // Si es nulo, retornar una lista vacía
            if (reportes == null)
            {
                return new List<Reporte>();
            }

            // Agrupar registros por ID, concatenando instrumentos
            var reportesAgrupados = reportes
                .GroupBy(r => r.id)
                .Select(g => new Reporte
                {
                    id = g.Key,
                    usuario = g.First().usuario,
                    grupo = g.First().grupo,
                    fecha_dado = g.First().fecha_dado,
                    fecha_regreso = g.First().fecha_regreso,
                    retornado = g.First().retornado,
                    instrumento = string.Join(", ", g.Select(r => r.instrumento).Distinct())
                })
                .ToList();

            return reportesAgrupados;
        }

        public async Task<byte[]> GenerarExcel(
           string ordenarPor,
           string direccion,
           string? filtroUsuario,
           string? filtroGrupo,
           string? filtroRetornado,
           string? filtroInstrumento, 
           string? fechaDesde,
            string? fechaHasta)
        {
            var reportes = await ObtenerRegistros();

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(filtroUsuario))
                reportes = reportes.Where(r => r.usuario.Contains(filtroUsuario, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroGrupo))
                reportes = reportes.Where(r => r.grupo != null && r.grupo.Contains(filtroGrupo, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroRetornado))
                reportes = reportes.Where(r => r.retornado.Equals(filtroRetornado, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroInstrumento))
                reportes = reportes.Where(r => r.instrumento.Contains(filtroInstrumento, StringComparison.OrdinalIgnoreCase)).ToList();

            var formatoFiltro = "yyyy-MM-dd";
            var formatoApartado = "dd/MM/yyyy"; 
            var cultura = CultureInfo.InvariantCulture;

            // Filtrar por rango de fechas (fecha_dado)
            if (DateTime.TryParseExact(fechaDesde, formatoFiltro, cultura, DateTimeStyles.None, out DateTime desde))
            {
                reportes = reportes
                    .Where(r => DateTime.TryParseExact(r.fecha_dado, formatoApartado, cultura, DateTimeStyles.None, out var f) && f >= desde)
                    .ToList();
            }

            if (DateTime.TryParseExact(fechaHasta, formatoFiltro, cultura, DateTimeStyles.None, out DateTime hasta))
            {
                reportes = reportes
                    .Where(r => DateTime.TryParseExact(r.fecha_dado, formatoApartado, cultura, DateTimeStyles.None, out var f) && f <= hasta)
                    .ToList();
            }
            
           

            // Aplicar ordenamiento
            reportes = ordenarPor switch
            {
                "usuario" => direccion == "asc" ? reportes.OrderBy(r => r.usuario).ToList() : reportes.OrderByDescending(r => r.usuario).ToList(),
                "fecha_dado" => direccion == "asc" ? reportes.OrderBy(r => r.fecha_dado).ToList() : reportes.OrderByDescending(r => r.fecha_dado).ToList(),
                "fecha_regreso" => direccion == "asc" ? reportes.OrderBy(r => r.fecha_regreso).ToList() : reportes.OrderByDescending(r => r.fecha_regreso).ToList(),
                "retornado" => direccion == "asc" ? reportes.OrderBy(r => r.retornado).ToList() : reportes.OrderByDescending(r => r.retornado).ToList(),
                _ => reportes
            };

            // Crear el archivo Excel en memoria
            ExcelPackage.License.SetNonCommercialPersonal("Armoniza");

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Reportes");

            // Cabeceras
            ws.Cells["B1"].Value = "Usuario";
            ws.Cells["C1"].Value = "Grupo";
            ws.Cells["D1"].Value = "Fecha de Apartado";
            ws.Cells["E1"].Value = "Fecha de Regreso";
            ws.Cells["F1"].Value = "Retornado";
            ws.Cells["G1"].Value = "Instrumento";

            using (var headerRange = ws.Cells["B1:G1"])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Datos
            int row = 2;
            foreach (var r in reportes)
            {
                ws.Cells[row, 2].Value = r.usuario;
                ws.Cells[row, 3].Value = string.IsNullOrEmpty(r.grupo) ? "—" : r.grupo;
                ws.Cells[row, 4].Value = r.fecha_dado;
                ws.Cells[row, 5].Value = r.fecha_regreso;
                ws.Cells[row, 6].Value = r.retornado.ToLower() != "pendiente" ? r.retornado.ToLower() : "Pendiente";
                ws.Cells[row, 7].Value = r.instrumento;
                row++;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray(); // Devolver el archivo en memoria
        }
    }

}



