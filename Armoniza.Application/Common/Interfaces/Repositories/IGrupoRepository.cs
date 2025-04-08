using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Domain.Entities;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public interface IGrupoRepository : IRepository<grupo>
    {
        Task<bool> Delete(grupo grupo);
    }
    
    
}
