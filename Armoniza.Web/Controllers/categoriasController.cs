using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;

namespace Armoniza.Web.Controllers
{
    public class categoriasController : Controller
    {
        private readonly ApplicationDbContext _context;


        public categoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: categorias
        public async Task<IActionResult> Index()
        {
            var categorias = await _context.categoria.Where(c => c.eliminado == false).ToListAsync();
            return View(categorias);
        }

        // GET: categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.categoria
                .FirstOrDefaultAsync(m => m.id == id);
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
                categoria.eliminado = false;
                _context.Add(categoria);
                await _context.SaveChangesAsync();
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

            var categoria = await _context.categoria.FindAsync(id);
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
                    categoria.eliminado = false;
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "¡Categoria actualizada exitosamente!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!categoriaExists(categoria.id))
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

            var categoria = await _context.categoria
                .FirstOrDefaultAsync(m => m.id == id);
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
            var categoria = await _context.categoria.FindAsync(id);
            if (categoria != null)
            {
                categoria.eliminado = true;
                _context.categoria.Update(categoria);
                TempData["success"] = "¡Categoria eliminada exitosamente!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool categoriaExists(int id)
        {
            return _context.categoria.Any(e => e.id == id);
        }
    }
}
