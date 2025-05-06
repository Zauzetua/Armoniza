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

            //Cargar instrumentos que pidio
            if (apartado.Data.idusuario == 0)
            {
                TempData["Error"] = "El apartado no existe.";
                return RedirectToAction("Index");
            }
            // Cargar instrumentos que pidio
            var instrumentos = _instrumentoService.GetAll(x => !x.eliminado && x.ocupado == true);
            // Filtrar los instrumentos ocupados por el usuario
            var instrumentosids = _apartadoService.GetInstrumentosPorUsuario(apartado.Data.idusuario);
            if (instrumentosids.Data != null)
            {
                instrumentos.Data = instrumentos.Data.Where(i => instrumentosids.Data.Any(x => x.id_instrumento == i.codigo)).ToList();
            }

            var ApartadoViewodel = new ApartadoViewModel
            {
                apartado = apartado.Data,
                instrumentos = instrumentos.Data,
            };


            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto", apartado.Data.idusuario);
            return View(ApartadoViewodel);
        }

        public IActionResult Create()
        {
            var usuarios = _usuarioService.GetAll(x => x.eliminado == false);
            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto");
            // VM vacío: sin sección de instrumentos aún
            return View(new ApartadoViewModel());
        }


        [HttpGet]
        public IActionResult Create(int idUsuario)
        {
            // 1) Cargo usuarios para el dropdown (el modal volverá a usar ViewData)
            var usuarios = _usuarioService.GetAll(x => x.eliminado == false);
            ViewData["idUsuario"] = new SelectList(usuarios.Data, "id", "nombreCompleto", idUsuario);

            // 2) Creo el VM y precargo:
            var vm = new ApartadoViewModel
            {
                apartado = new apartado { idusuario = idUsuario },
                instrumentos = _instrumentoService.GetAll(x => !x.eliminado && !x.ocupado).Data,
                MaxInstrumentos = _usuarioService.ObtenerMaximoInstrumentos(idUsuario).Result.Data

            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(ApartadoViewModel vm)
        {
            // re-subimos instrumentos completo para redisplay si falla
            vm.instrumentos = _instrumentoService.GetAll(x => !x.eliminado).Data;
            // y re-calculamos el límite
            vm.MaxInstrumentos = _usuarioService.ObtenerMaximoInstrumentos(vm.apartado.idusuario).Result.Data;

            vm.instrumentosSeleccionados = vm.instrumentosSeleccionados.Where(i => i > 0).ToList();

            if (ModelState.IsValid)
            {
                var result = await _apartadoService.CrearApartado(vm);
                if (result.Success)
                    return RedirectToAction(nameof(Index));
                TempData["Error"] = result.Message;
                ModelState.AddModelError("", result.Message);
            }

            ViewData["idUsuario"] = new SelectList(
                _usuarioService.GetAll(x => !x.eliminado).Data,
                "id", "nombreCompleto",
                vm.apartado.idusuario);

            return View(vm);
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
