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
    public interface IInstrumentoService
    {
        Task<ServiceResponse<bool>> Delete(int codigo);
        Task<ServiceResponse<instrumento>> Add(instrumento instrumento);
        ServiceResponse<IEnumerable<instrumento>> GetAll(Expression<Func<instrumento, bool>> filter);
        ServiceResponse<IEnumerable<instrumento>> GetAll();
        ServiceResponse<instrumento> Get(Expression<Func<instrumento, bool>> filter);
        ServiceResponse<bool> Update(instrumento instrumento);
        ServiceResponse<bool> Any(Expression<Func<instrumento, bool>> filter, string? includePropierties = null);
        ServiceResponse<bool> Ocupar(int codigo);
        ServiceResponse<bool> Desocupar(int codigo);
        ServiceResponse<bool> Roto(int codigo);
        ServiceResponse<bool> Arreglado(int codigo);


    }
}
