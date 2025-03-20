using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;

namespace Armoniza.Web.Controllers
{
    public class categoriasController : Controller
    {
        private readonly ICategoriasService<categoria> _categoriasService;

        public categoriasController(ICategoriasService<categoria> _categoriasService)
        {
            this._categoriasService = _categoriasService;
        }

        // GET: categorias
        public async Task<IActionResult> Index()
        {
            var categorias =  await _categoriasService.GetAll(u => u.eliminado ==false);
            return View(categorias);
        }

        // GET: categorias/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria =  await _categoriasService.Get(u => u.id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: categorias/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("categoria1,eliminado,id")] categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriasService.Add(categoria);
                TempData["success"] = "¡Categoria creada exitosamente!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "¡Error al crear la categoria!";
            return View(categoria);
        }

        // GET: categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoriasService.Get(u => u.id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("categoria1,eliminado,id")] categoria categoria)
        {
            if (id != categoria.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriasService.Update(categoria);
                    TempData["success"] = "¡Categoria actualizada exitosamente!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _categoriasService.Any(e => e.id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "¡Error al actualizar la categoria!";
            return View(categoria);
        }

        // GET: categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _categoriasService.Get(u => u.id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultado = await _categoriasService.Delete(id);
            if (resultado)
            {
                TempData["success"] = "¡Categoria eliminada exitosamente!";
            }
            else
            {
                TempData["error"] = "¡Error al eliminar la categoria!";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> categoriaExists(int id)
        {
            return await _categoriasService.Any(e => e.id == id);
        }
    }
}
