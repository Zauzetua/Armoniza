using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Infrastructure.Services
{
    public class CategoriasService : ICategoriasService<categoria>
    {
        private readonly ICategoriasRepository _categoriasRepository;

        public CategoriasService(ICategoriasRepository categoriasRepository)
        {
            _categoriasRepository = categoriasRepository;
        }
        public Task<bool> Add(categoria categoria)
        {
            if (categoria is not null)
            {
                categoria.eliminado = false;
                _categoriasRepository.Add(categoria);
                _categoriasRepository.save();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> Delete(int id)
        {

            var categoria = _categoriasRepository.Get(x => x.id == id);
            if (categoria is not null)
            {
                categoria.eliminado = true;
                _categoriasRepository.Update(categoria);
                _categoriasRepository.save();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }


        public Task<bool> Edit(int id)
        {

            var categoria = _categoriasRepository.Get(x => x.id == id);
            if (categoria is not null)
            {
                categoria.eliminado = false;
                _categoriasRepository.Update(categoria);
                _categoriasRepository.save();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<categoria>> GetAll(System.Linq.Expressions.Expression<Func<categoria, bool>> filter)
        {
            var categorias = _categoriasRepository.GetAll(filter);
            return Task.FromResult(categorias);
        }

        public Task<IEnumerable<categoria>> GetAll()
        {
            var categorias = _categoriasRepository.GetAll();
            return Task.FromResult(categorias);
        }

        public Task<categoria> Get(System.Linq.Expressions.Expression<Func<categoria, bool>> filter)
        {
            var categorias = _categoriasRepository.Get(filter);
            return Task.FromResult(categorias);
        }

        public Task<bool> Update(categoria categoria)
        {
            if (categoria is not null)
            {
                _categoriasRepository.Update(categoria);
                _categoriasRepository.save();
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> Any(System.Linq.Expressions.Expression<Func<categoria, bool>> filter, string? includePropierties = null)
        {
            return Task.FromResult(_categoriasRepository.Any(filter, includePropierties));
        }
    }
}
