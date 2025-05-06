using System.Security.Claims;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Infrastructure.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Armoniza.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService<Admin> _accountService;

        public AccountController(IAccountService<Admin> accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]  
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]  
        public async Task<IActionResult> Login(string username, string password)
        {
            var result =  await _accountService.Login(username, password);
            if (result)
            {
                TempData["Success"] = "Inicio de sesion correcto";
                return RedirectToAction("Index", "Home");
            }
            TempData["Error"] = "Usuario o contraseña incorrectos";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            _accountService.LogOut();
            return RedirectToAction("Login");
        }
    }
}
