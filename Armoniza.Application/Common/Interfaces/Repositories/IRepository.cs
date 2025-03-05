using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        public T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);

        bool Any(Expression<Func<T, bool>> filter, string? includePropierties = null);

    }
}
