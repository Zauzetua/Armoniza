using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Infrastructure.Infrastructure.Data;

namespace Armoniza.Infrastructure.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;



        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
