using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public interface ICategoriasRepository : IRepository<categoria>
    {
        Task<bool> Delete(categoria categoria);
    }
}
