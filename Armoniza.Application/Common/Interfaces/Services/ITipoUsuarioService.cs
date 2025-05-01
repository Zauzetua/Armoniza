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
    public interface ITipoUsuarioService
    {
        ServiceResponse<bool> Delete(int id);
        Task<ServiceResponse<bool>> Add(tipoUsuario instrumento);
        ServiceResponse<IEnumerable<tipoUsuario>> GetAll(Expression<Func<tipoUsuario, bool>> filter);
        ServiceResponse<IEnumerable<tipoUsuario>> GetAll();
        ServiceResponse<tipoUsuario> Get(Expression<Func<tipoUsuario, bool>> filter);
        ServiceResponse<bool> Update(tipoUsuario instrumento);
        ServiceResponse<bool> Any(Expression<Func<tipoUsuario, bool>> filter, string? includePropierties = null);

    }
}
