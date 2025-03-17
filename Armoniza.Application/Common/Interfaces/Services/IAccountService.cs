using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Services
{
    public interface IAccountService<T> where T : class
    {
        Task<bool> Login(string username,string password);
        void LogOut();

    }
}
