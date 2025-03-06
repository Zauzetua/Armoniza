using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Armoniza.Application.Common.Interfaces.Services
{
    public interface IAuthService
    {
         Task<bool> LoginAsync(string username, string password, HttpContext httpContext);
         Task LogoutAsync(HttpContext httpContext);
    }
}
