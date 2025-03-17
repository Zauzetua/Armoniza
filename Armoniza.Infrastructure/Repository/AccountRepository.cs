using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Infrastructure.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Infrastructure.Repository
{
    public class AccountRepository : Repository<Admin>, IAccountRepository<Admin>
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void FindByCondition(Func<Admin, bool> func)
        {
            
            var admin = _context.Admin.FirstOrDefault(func);

        }

        public  Admin Get(string username)
        {
            var admin =  _context.Admin.FirstOrDefault(x => x.username == username);
            if (admin == null)
            {
                return null;
            }
            return admin;
        }

        public List<Admin> GetAll()
        {
            return _context.Admin.ToList();
        }

    }   
    
    
}
