using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

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
    string? filtroInstrumento = null)
        {
            var reportes = _reportesService.ObtenerRegistros().Result;

            if (reportes is null)
            {
                TempData["error"] = "¡Error al obtener los reportes!";
                return RedirectToAction("Index", "Home");
            }

            // Aplicar filtros
            if (!string.IsNullOrWhiteSpace(filtroUsuario))
                reportes = reportes.Where(r => r.usuario.Contains(filtroUsuario, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroGrupo))
                reportes = reportes.Where(r => r.grupo != null && r.grupo.Contains(filtroGrupo, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroRetornado))
                reportes = reportes.Where(r => r.retornado.Equals(filtroRetornado, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrWhiteSpace(filtroInstrumento))
                reportes = reportes.Where(r => r.instrumento.Contains(filtroInstrumento, StringComparison.OrdinalIgnoreCase)).ToList();

            // Ordenar
            reportes = ordenarPor switch
            {
                "usuario" => direccion == "asc" ? reportes.OrderBy(r => r.usuario).ToList() : reportes.OrderByDescending(r => r.usuario).ToList(),
                "fecha_dado" => direccion == "asc" ? reportes.OrderBy(r => r.fecha_dado).ToList() : reportes.OrderByDescending(r => r.fecha_dado).ToList(),
                "fecha_regreso" => direccion == "asc" ? reportes.OrderBy(r => r.fecha_regreso).ToList() : reportes.OrderByDescending(r => r.fecha_regreso).ToList(),
                "retornado" => direccion == "asc" ? reportes.OrderBy(r => r.retornado).ToList() : reportes.OrderByDescending(r => r.retornado).ToList(),
                _ => reportes
            };

            var vm = new ReporteViewModel
            {
                Reportes = reportes,
                OrdenarPor = ordenarPor,
                Direccion = direccion,
                FiltroUsuario = filtroUsuario,
                FiltroGrupo = filtroGrupo,
                FiltroRetornado = filtroRetornado,
                FiltroInstrumento = filtroInstrumento
            };

            return View(vm);
        }


    }
}
