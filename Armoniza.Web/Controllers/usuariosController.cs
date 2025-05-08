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
using Microsoft.AspNetCore.Authorization;

namespace Armoniza.Web.Controllers
{
	
	public class usuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IGrupoService _grupoService;
        private readonly ITipoUsuarioService _tipoUsuarioService;

        public usuariosController(IUsuarioService usuarioService, IGrupoService grupoService, ITipoUsuarioService tipoUsuarioService)
        {
            _usuarioService = usuarioService;
            _grupoService = grupoService;
            _tipoUsuarioService = tipoUsuarioService;

        }

        // GET: usuarios
        public IActionResult Index()
        {
            var usuarios = _usuarioService.GetAll(u => u.eliminado == false);

            ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
            return View(usuarios.Data);

        }

        // GET: usuarios/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "¡Usuario no encontrado!";
                return RedirectToAction(nameof(Index));
            }

            var usuario = _usuarioService.Get(u => u.id == id);
            if (usuario.Data == null)
            {
                TempData["Error"] = "¡Usuario no encontrado!";
                return RedirectToAction(nameof(Index));
            }

            ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
            return View(usuario.Data);
        }

        // GET: usuarios/Create
        public IActionResult Create()
        {
            var grupos = _grupoService.GetAll(g => g.eliminado == false).Data
            .Select(g => new { g.id, g.grupo1 });

            ViewData["idGrupo"] = new SelectList(grupos, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("id,nombreCompleto,telefono,correo,idTipo,idGrupo,eliminado")] usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var response = _usuarioService.Add(usuario);
                if (response.Success)
                {
                    TempData["Success"] = "¡Usuario agregado exitosamente!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = response.Message;
					ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
					ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
					return View(usuario);
                }
            }
            TempData["Error"] = "Error: ¡Datos no validos!";
            ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
            return View(usuario);
        }

        // GET: usuarios/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "¡Usuario no encontrado!";
                return RedirectToAction(nameof(Index));
            }

            var usuario = _usuarioService.Get(u => u.id == id);
            if (usuario.Data == null)
            {
                TempData["Error"] = "¡Usuario no encontrado!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
            return View(usuario.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, usuario usuario)
        {
            if (id != usuario.id)
            {
                TempData["Error"] = "¡Usuario no encontrado!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var response = _usuarioService.Update(usuario);
                if (response.Success)
                {
                    TempData["Success"] = "¡Usuario actualizado exitosamente!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Error"] = response.Message;
					ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
					ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
					return View(usuario);
                }
            }
            TempData["Error"] = "Error: ¡Datos incorrectos!";
            ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
            return View(usuario);
        }

        // GET: usuarios/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Error"] = "¡Usuario no encontrado!";
                return RedirectToAction(nameof(Index));
            }

            var usuario = _usuarioService.Get(u => u.id == id);
            if (usuario.Data == null)
            {
                TempData["Error"] = "¡Usuario no encontrado!";

                return RedirectToAction(nameof(Index));
            }

            ViewData["idGrupo"] = new SelectList(_grupoService.GetAll(g => g.eliminado == false).Data, "id", "grupo1");
            ViewData["idTipo"] = new SelectList(_tipoUsuarioService.GetAll(t => t.eliminado == false).Data, "id", "tipo");
            return View(usuario.Data);
        }

        // POST: usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = _usuarioService.Get(u => u.id == id);
            if (usuario == null)
            {
                TempData["Error"] = "¡Usuario no encontrado!";
                return RedirectToAction(nameof(Index));
            }
            var response = await _usuarioService.Delete(id);
            if (response.Success)
            {
                TempData["Success"] = "¡Usuario eliminado exitosamente!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Error"] = response.Message;
                return RedirectToAction(nameof(Index));

            }
            return View(usuario.Data);

        }


    }
}
