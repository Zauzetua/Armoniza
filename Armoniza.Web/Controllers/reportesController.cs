using System.Globalization;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Armoniza.Web.Controllers
{
    public class reportesController : Controller
    {
        private readonly IReportesService _reportesService;
        public reportesController(IReportesService reportesService)
        {
            _reportesService = reportesService;
        }


        public IActionResult Index(
        string ordenarPor = "fecha_dado",
        string direccion = "asc",
        string? filtroUsuario = null,
        string? filtroGrupo = null,
        string? filtroRetornado = null,
        string? filtroInstrumento = null,
        string? fechaDesde = null,
        string? fechaHasta = null,
        string? filtroDevuelto = null,
        int pagina = 1,
        int tamanoPagina = 10)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Armoniza");
            // O LicenseContext.NonCommercial si es una versión no comercial

            var reportes = _reportesService.ObtenerRegistros().Result;

            if (reportes is null)
            {
                TempData["error"] = "¡Error al obtener los reportes!";
                return RedirectToAction("Index", "Home");
            }

            // Filtrar
            if (!string.IsNullOrWhiteSpace(filtroUsuario))
                reportes = reportes.Where(r => r.usuario.Contains(filtroUsuario, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroGrupo))
                reportes = reportes.Where(r => r.grupo != null && r.grupo.Contains(filtroGrupo, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroRetornado))
                reportes = reportes.Where(r => r.retornado.Equals(filtroRetornado, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroInstrumento))
                reportes = reportes.Where(r => r.instrumento.Contains(filtroInstrumento, StringComparison.OrdinalIgnoreCase)).ToList();

            var formatoFiltro = "yyyy-MM-dd";
            var formatoApartado = "dd/MM/yyyy"; // o "dd-MM-yyyy" si ese es el que usas
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

            // Filtrar por devueltos o no
            if (!string.IsNullOrEmpty(filtroDevuelto))
            {
                if (filtroDevuelto == "Sí")
                    reportes = reportes.Where(r => !r.retornado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase)).ToList();
                else if (filtroDevuelto == "No")
                    reportes = reportes.Where(r => r.retornado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase)).ToList();
            }
            // Ordenar
            reportes = ordenarPor switch
            {
                "usuario" => direccion == "asc" ? reportes.OrderBy(r => r.usuario).ToList() : reportes.OrderByDescending(r => r.usuario).ToList(),
                "fecha_dado" => direccion == "asc" ? reportes.OrderBy(r => r.fecha_dado).ToList() : reportes.OrderByDescending(r => r.fecha_dado).ToList(),
                "fecha_regreso" => direccion == "asc" ? reportes.OrderBy(r => r.fecha_regreso).ToList() : reportes.OrderByDescending(r => r.fecha_regreso).ToList(),
                "retornado" => direccion == "asc" ? reportes.OrderBy(r => r.retornado).ToList() : reportes.OrderByDescending(r => r.retornado).ToList(),
                _ => reportes
            };

            int totalRegistros = reportes.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanoPagina);

            var reportesPaginados = reportes
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            var vm = new ReporteViewModel
            {
                Reportes = reportesPaginados,
                OrdenarPor = ordenarPor,
                Direccion = direccion,
                FiltroUsuario = filtroUsuario,
                FiltroGrupo = filtroGrupo,
                FiltroRetornado = filtroRetornado,
                FiltroInstrumento = filtroInstrumento,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                FiltroDevuelto = filtroDevuelto
            };

            return View(vm);
        }

        public async Task<IActionResult> ExportarExcel(
        string ordenarPor = "fecha_dado",
        string direccion = "asc",
        string? filtroUsuario = null,
        string? filtroGrupo = null,
        string? filtroRetornado = null,
        string? filtroInstrumento = null,
        string? fechaDesde = null,
        string? fechaHasta = null,
        string? filtroDevuelto = null)
        {
            var excelFile = await _reportesService.GenerarExcel(
                ordenarPor, direccion, filtroUsuario, filtroGrupo, filtroRetornado, filtroInstrumento, fechaDesde, fechaHasta, filtroDevuelto);

            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reportes.xlsx");
        }

    }
}
