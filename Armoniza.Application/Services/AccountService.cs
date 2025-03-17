using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Armoniza.Application.Services
{
    public class AccountService : IAccountService<Admin>
    {
        private readonly IAccountRepository<Admin> _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AccountService(IAccountRepository<Admin> accountRepository, IHttpContextAccessor httpContextAccessor)
        {
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Login(string username, string password)
        {
            var admin = _accountRepository.Get(username);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.password))
            {
                return false;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.username)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            if (principal.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        public async void LogOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        }
    }
}
