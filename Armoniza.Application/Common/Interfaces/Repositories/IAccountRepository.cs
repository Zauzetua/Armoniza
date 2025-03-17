using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public interface IAccountRepository<T> where T : class
    {
        T Get(string username);
    }
}
