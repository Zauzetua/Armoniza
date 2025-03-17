using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Infrastructure.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;


namespace Armoniza.Application.Services
{
    public class AccountService : IAccountService<Admin>
    {
        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public AccountService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<bool> Login(string username, string password)
        {
            var admin = _context.Admin.FirstOrDefault(x => x.username == username);

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
