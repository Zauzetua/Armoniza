using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public interface IAccountRepository<T> where T : class
    {
        void FindByCondition(Func<Admin, bool> func);
        T Get(string username);
        List<T> GetAll();
    }
}
