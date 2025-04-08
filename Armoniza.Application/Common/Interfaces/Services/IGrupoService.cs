using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Models;
using Armoniza.Domain.Entities;

namespace Armoniza.Application.Common.Interfaces.Services
{
    public interface IGrupoService
    {
        Task<ServiceResponse<grupo>> Delete(int id);
        ServiceResponse<grupo> Add(grupo grupo);
        ServiceResponse<IEnumerable<grupo>> GetAll(Expression<Func<grupo, bool>> filter);
        ServiceResponse<IEnumerable<grupo>> GetAll();
        Task<ServiceResponse<grupo>> Get(Expression<Func<grupo, bool>> filter);
        Task<ServiceResponse<grupo>> Update(grupo grupo);
        Task<ServiceResponse<grupo>> Any(Expression<Func<grupo, bool>> filter, string? includePropierties = null);


    }
}
