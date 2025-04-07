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
    public class usuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public usuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: usuarios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.usuarios.Include(u => u.idGrupoNavigation).Include(u => u.idTipoNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .Include(u => u.idGrupoNavigation)
                .Include(u => u.idTipoNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: usuarios/Create
        public IActionResult Create()
        {
            ViewData["idGrupo"] = new SelectList(_context.grupos, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_context.tipoUsuarios, "id", "tipo");
            return View();
        }

        // POST: usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombreCompleto,telefono,correo,idTipo,idGrupo,eliminado")] usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idGrupo"] = new SelectList(_context.grupos, "id", "grupo1", usuario.idGrupo);
            ViewData["idTipo"] = new SelectList(_context.tipoUsuarios, "id", "tipo", usuario.idTipo);
            return View(usuario);
        }

        // GET: usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["idGrupo"] = new SelectList(_context.grupos, "id", "grupo1", usuario.idGrupo);
            ViewData["idTipo"] = new SelectList(_context.tipoUsuarios, "id", "tipo", usuario.idTipo);
            return View(usuario);
        }

        // POST: usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, usuario usuario)
        {
            if (id != usuario.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!usuarioExists(usuario.id))
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
            ViewData["idGrupo"] = new SelectList(_context.grupos, "id", "grupo1", usuario.idGrupo);
            ViewData["idTipo"] = new SelectList(_context.tipoUsuarios, "id", "tipo", usuario.idTipo);
            return View(usuario);
        }

        // GET: usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.usuarios
                .Include(u => u.idGrupoNavigation)
                .Include(u => u.idTipoNavigation)
                .FirstOrDefaultAsync(m => m.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool usuarioExists(int id)
        {
            return _context.usuarios.Any(e => e.id == id);
        }
    }
}
