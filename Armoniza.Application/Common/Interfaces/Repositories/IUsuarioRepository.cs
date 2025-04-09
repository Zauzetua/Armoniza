using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<usuario>
    {
        Task<bool> Delete(usuario usuario);
    }
    
    
}
