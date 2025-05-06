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
using Microsoft.AspNetCore.Authorization;

namespace Armoniza.Web.Controllers
{
    
    public class instrumentosController : Controller
    {
        private readonly IInstrumentoService _instrumentoService;
        private readonly ICategoriasService<categoria> _categoriasService;

        public instrumentosController(IInstrumentoService instrumentoService, ICategoriasService<categoria> categoriasService)
        {
            _instrumentoService = instrumentoService;
            _categoriasService = categoriasService;
        }

        // GET: instrumentos
        public IActionResult Index()
        {
            
            var instrumentos = _instrumentoService.GetAll(i => !i.eliminado).Data;

            
            var categorias = _categoriasService.GetAll(c => !c.eliminado).Result;

            
            List<InstrumentosViewModel> modelo = new List<InstrumentosViewModel>();

            
            foreach (var categoria in categorias)
            {
                
                var instrumentosDeCategoria = instrumentos
                    .Where(i => i.idCategoria == categoria.id)
                    .ToList();

               

                modelo.Add(new InstrumentosViewModel
                {
                    IdCategoria = categoria.id,
                    NombreCategoria = categoria.categoria1,
                    Instrumentos = instrumentosDeCategoria
                });

            }

            
            return View(modelo);
        }



        // GET: instrumentos/Details/5
        public  IActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }

            var instrumento = _instrumentoService.Get(i => i.codigo == id);
            if (instrumento.Data == null)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }
            var categorias = _categoriasService.GetAll(c => c.eliminado == false).Result;
            ViewData["idCategoria"] = new SelectList(categorias.ToList(), "id", "categoria1");
            return View(instrumento.Data);
        }

        // GET: instrumentos/Create
        public async Task<IActionResult> Create()
        {
            var categorias = await _categoriasService.GetAll(c => c.eliminado == false);
            ViewData["idCategoria"] = new SelectList(categorias, "id", "categoria1");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("codigo,estuche,ocupado,funcional,eliminado,idCategoria,nombre")] instrumento instrumento)
        {
            if (ModelState.IsValid)
            {
                var response = await _instrumentoService.Add(instrumento);
                if (response.Success)
                {
                    TempData["success"] = "¡Instrumento creado correctamente!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction(nameof(Create));
                }
            }
            TempData["error"] = "¡Error al crear el instrumento!";
            var categorias = await _categoriasService.GetAll(c => c.eliminado == false);
            ViewData["idCategoria"] = new SelectList(categorias, "id", "categoria1", instrumento.idCategoria);
            return View(instrumento);
        }

        // GET: instrumentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }

            var instrumento = _instrumentoService.Get(i => i.codigo == id);
            if (instrumento.Data == null)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }
            var categorias = await _categoriasService.GetAll(c => c.eliminado == false);
            ViewData["idCategoria"] = new SelectList(categorias, "id", "categoria1", instrumento.Data.idCategoria);
            return View(instrumento.Data);
        }

        // POST: instrumentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("codigo,estuche,ocupado,funcional,eliminado,idCategoria,nombre")] instrumento instrumento)
        {
            if (id != instrumento.codigo)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var response = _instrumentoService.Update(instrumento);
                if (response.Success)
                {
                    TempData["success"] = "¡Instrumento editado correctamente!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["error"] = "¡Error al editar el instrumento!";
            var categorias = await _categoriasService.GetAll(c => c.eliminado == false);
            ViewData["idCategoria"] = new SelectList(categorias, "id", "categoria1", instrumento.idCategoria);
            return View(instrumento);
        }

        // GET: instrumentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }

            var instrumento = _instrumentoService.Get(i => i.codigo == id);
            if (instrumento.Data == null)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }
            var categorias = _categoriasService.GetAll(c => c.eliminado == false).Result;
            ViewData["idCategoria"] = new SelectList(categorias.ToList(), "id", "categoria1");
            return View(instrumento.Data);
        }

        // POST: instrumentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instrumento = _instrumentoService.Get(i => i.codigo == id);
            if (instrumento.Data == null)
            {
                TempData["error"] = "¡No se encontro este instrumento!";
                return RedirectToAction(nameof(Index));
            }
            var response = await _instrumentoService.Delete(id);
            if (response.Success)
            {
                TempData["success"] = "¡Instrumento eliminado correctamente!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }
        }




    }
}
