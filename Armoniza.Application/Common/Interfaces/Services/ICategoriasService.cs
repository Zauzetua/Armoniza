using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Services
{
    public interface ICategoriasService<T> where T : class
    {
        Task<bool> Delete(int id);
        Task<bool> Edit(int id);
        Task<bool> Add(categoria categoria);
       
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(Expression<Func<T, bool>> filter);

        Task<bool> Update(categoria categoria);

        Task<bool> Any(Expression<Func<T, bool>> filter, string? includePropierties = null);

    }
}
