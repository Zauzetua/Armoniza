using Armoniza.Application.Common.Interfaces.Services;
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
        public IActionResult Index()
        {
            var reportes = _reportesService.ObtenerRegistros().Result;
            if (reportes is not null)
            {
                return View(reportes);
            }
            else
            {
                TempData["error"] = "¡Error al obtener los reportes!";
                return RedirectToAction("Index", "Home");
            }
            
        }
    }
}
