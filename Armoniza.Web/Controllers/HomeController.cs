using System.Diagnostics;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Domain.Entities;
using Armoniza.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Armoniza.Web.Controllers
{
	
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApartadosService _apartadosService;
        private readonly IUsuarioService _usuarioService;

        public HomeController(ILogger<HomeController> logger, IApartadosService apartadosService, IUsuarioService usuarioService)
        {
            _logger = logger;
            _apartadosService = apartadosService;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index(DateTime? fechaLimite)
        {
            var hoy = DateTime.Today;
            var limite = fechaLimite ?? hoy.AddDays(7); // Por defecto, próximos 7 días

            var apartadosProximos = _apartadosService.GetAll(a =>
                    a.fecharegreso >= hoy &&
                    a.fecharegreso <= limite &&
                    a.activo)
                .Data
                .OrderBy(a => a.fecharegreso)
                .ToList();

            var usuarios = _usuarioService.GetAll(x => x.eliminado == false);
            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto");
            ViewBag.FechaLimite = limite.ToString("yyyy-MM-dd"); // Para usar en la vista
            return View(apartadosProximos);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
