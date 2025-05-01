using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Application.Common.Models;

namespace Armoniza.Web.Controllers
{
    public class apartadoesController : Controller
    {
        private readonly IApartadosService _apartadoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IInstrumentoService _instrumentoService;

        public apartadoesController(IApartadosService apartadosService, IUsuarioService usuarioService, IInstrumentoService instrumentoService)
        {
            _apartadoService = apartadosService;
            _usuarioService = usuarioService;
            _instrumentoService = instrumentoService;
        }

        // GET: apartadoes
        public IActionResult Index()
        {
            var apartados = _apartadoService.GetAll(a => a.activo);
            var usuarios = _usuarioService.GetAll(x => x.eliminado == false);
            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto");
            return View(apartados.Data);

        }

        // GET: apartadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "El apartado no existe.";
                RedirectToAction("Index");
            }

            var apartado = _apartadoService.Get(x => x.id == id);
            if (apartado.Data == null)
            {
                TempData["Error"] = "El apartado no existe.";
                return RedirectToAction("Index");
            }
            var usuarios = _usuarioService.GetAll(x => x.eliminado == false);
            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto");
            return View(apartado.Data);
        }

        // GET: apartadoes/Create
        public IActionResult Create()
        {
            var usuarios = _usuarioService.GetAll(x => x.eliminado == false);
            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto");
            var instrumentos = _instrumentoService.GetAll(x => x.eliminado == false);
            ViewData["instrumentos"] = new SelectList(instrumentos.Data, "codigo", "nombre");
            return View();

        }

        // POST: apartadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApartadoViewModel ApartadoModelo)
        {
            var instrumentoDummy = _instrumentoService.GetAll(x => x.eliminado == false);
            ApartadoModelo.instrumentos = instrumentoDummy.Data;
            if (ModelState.IsValid)
            {
                var result = await _apartadoService.CrearApartado(ApartadoModelo);
                if (result.Success)
                {
                    TempData["Success"] = "El apartado se ha creado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = result.Message;
                    ModelState.AddModelError("", result.Message);
                }
            }
            // Si el modelo no es válido, volvemos a cargar la lista de usuarios
            TempData["Error"] = "Error al crear el apartado.";
            var usuarios = _usuarioService.GetAll(x => x.eliminado == false);
            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto", ApartadoModelo.apartado.idusuario);
            var instrumentos = _instrumentoService.GetAll(x => x.eliminado == false);
            ViewData["instrumentos"] = new SelectList(instrumentos.Data, "codigo", "nombre");
            return View(ApartadoModelo);
        }

        // GET: apartadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "El apartado no existe.";
                return RedirectToAction("Index");
            }

            var apartado = _apartadoService.Get(x => x.id == id);
            if (apartado.Data == null)
            {
                TempData["Error"] = "El apartado no existe.";
                return RedirectToAction("Index");
            }
            // Cargar la lista de usuarios para el dropdown
            TempData["Error"] = "El apartado no existe.";
            ViewData["idUsuario"] = new SelectList(_apartadoService.GetAll().Data, "id", "nombreCompleto", apartado.Data.idusuario);
            return View(apartado.Data);
        }
        [HttpGet]
        public IActionResult LiberarApartado(int id)
        {
            var apartado = _apartadoService.Get(x => x.id == id);
            if (apartado.Data == null)
            {
                TempData["Error"] = "El apartado no existe.";
                return RedirectToAction("Index");
            }
            return View(apartado.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LiberarApartadoConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _apartadoService.LiberarApartado(id);
                if (result.Success)
                {
                    TempData["Success"] = "El apartado se ha liberado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }
            TempData["Error"] = "Error al liberar el apartado.";
            var apartado = _apartadoService.Get(x => x.id == id);
            return View(apartado);
        }


    }
}
