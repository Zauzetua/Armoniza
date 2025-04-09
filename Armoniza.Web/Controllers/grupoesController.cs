using Microsoft.AspNetCore.Mvc;
using Armoniza.Domain.Entities;
using Armoniza.Application.Common.Interfaces.Services;

namespace Armoniza.Web.Controllers
{
    public class grupoesController : Controller
    {
        private readonly IGrupoService _grupoService;

        public grupoesController(IGrupoService grupoService)
        {
            _grupoService = grupoService;
        }

        // GET: grupoes
        public IActionResult Index()
        {
            var grupos = _grupoService.GetAll(u => u.eliminado == false);
            return View(grupos.Data);
        }

        // GET: grupoes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "¡No se encontro este grupo!";
                return RedirectToAction(nameof(Index));
            }
            var grupo = _grupoService.Get(u => u.id == id);
            if (grupo.Success)
            {
                TempData["success"] = grupo.Message;
                return View(grupo.Data);
                
            }
            TempData["error"] = "¡No se encontro este grupo!";
            return RedirectToAction(nameof(Index));
        }

        // GET: grupoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: grupoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,grupo1,eliminado")] grupo grupo)
        {
            if (ModelState.IsValid)
            {
                _grupoService.Add(grupo);
                TempData["success"] = "¡Grupo agregado exitosamente!";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "¡Error al agregar el grupo!";
            return View(grupo);
        }

        // GET: grupoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "¡No se encontro este grupo!";
                return RedirectToAction(nameof(Index));
            }

            var grupo = _grupoService.Get(u => u.id == id);
            if (grupo.Data == null)
            {
                TempData["error"] = grupo.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(grupo.Data);
        }

        // POST: grupoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,grupo1,eliminado")] grupo grupo)
        {
            if (id != grupo.id)
            {
                TempData["error"] = "¡Error al obtener el grupo!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var grupoExistente = _grupoService.Get(u => u.id == id);
                if (grupoExistente.Data == null)
                {
                    TempData["error"] = grupoExistente.Message;
                    return RedirectToAction(nameof(Index));
                }
                _grupoService.Update(grupo);
                TempData["success"] = "¡Grupo editado exitosamente!";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "¡Error al editar el grupo!";
            return View(grupo);
        }

        // GET: grupoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "¡No se encontro este grupo!";
                return RedirectToAction(nameof(Index));
            }

            var grupo = _grupoService.Get(u => u.id == id);
            if (grupo.Data == null)
            {
                TempData["error"] = grupo.Message;
                return RedirectToAction(nameof(Index));
            }
            return View(grupo.Data);
        }

        // POST: grupoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = _grupoService.Get(u => u.id == id);
            if (!grupo.Success)
            {
                TempData["error"] = grupo.Message;
                return RedirectToAction(nameof(Index));
            }
            var grupoEliminado = await _grupoService.Delete(id);
            if (!grupoEliminado.Success)
            {
                TempData["error"] = grupoEliminado.Message;
                return RedirectToAction(nameof(Index));
            }
            TempData["success"] = "¡Grupo eliminado exitosamente!";
            return RedirectToAction(nameof(Index));

        }

       
    }
}
