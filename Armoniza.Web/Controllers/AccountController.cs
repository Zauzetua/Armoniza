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
           
            var admin = _context.Admin.SingleOrDefault(a => a.username == username);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.password))
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.username)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
