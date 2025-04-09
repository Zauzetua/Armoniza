using Armoniza.Application.Common.Models;
using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Services
{
    public interface IUsuarioService 
    {
        Task<ServiceResponse<usuario>> Delete(int id);
        ServiceResponse<usuario> Add(usuario usuario);
        ServiceResponse<IEnumerable<usuario>> GetAll(Expression<Func<usuario, bool>> filter);
        ServiceResponse<IEnumerable<usuario>> GetAll();
        ServiceResponse<usuario> Get(Expression<Func<usuario, bool>> filter);
        ServiceResponse<bool> Update(usuario grupo);
        ServiceResponse<bool> Any(Expression<Func<usuario, bool>> filter, string? includePropierties = null);
    }
}
