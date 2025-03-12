using System.Security.Claims;
using Armoniza.Infrastructure.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Armoniza.Web.Controllers
{
    public class AccountController : Controller
    {
        // Se inyecta el contexto de la base de datos
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]  // Evita que se requiera autenticación para esta vista
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]  //Permite acceder al método sin autenticación
        public async Task<IActionResult> Login(string username, string password)
        {
            // Se busca el usuario en la base de datos
            var admin = _context.Admin.SingleOrDefault(a => a.username == username);

            // Si el usuario no existe o la contraseña no coincide, se muestra un mensaje de error
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.password))
            {
                TempData["error"] = "Usuario o contraseña incorrectos."; // Se puede usar TempData para enviar mensajes entre vistas
                return View();
            }

            // Se crea una lista de claims con el nombre del usuario
            //Los claims son una forma de representar la identidad de un usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.username)
            };

            // Se crea un objeto ClaimsIdentity con los claims
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // Se crea un objeto ClaimsPrincipal con el objeto ClaimsIdentity
            var principal = new ClaimsPrincipal(identity);

            // Se inicia sesión con el objeto ClaimsPrincipal
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            TempData["success"] = "¡Inicio sesion exitosamente!"; // Se puede usar TempData para enviar mensajes entre vistas
            // Se redirige a la página de inicio
            return RedirectToAction("Index", "Home");


        }

        public async Task<IActionResult> Logout()
        {
            // Se cierra la sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
