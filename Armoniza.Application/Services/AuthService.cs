using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Armoniza.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        public Task<bool> LoginAsync(string username, string password, HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        public bool verifyPasswords(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

    }
}
