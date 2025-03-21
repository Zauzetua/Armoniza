using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Infrastructure.Repository
{
    public class CategoriasRepository : Repository<categoria>, ICategoriasRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriasRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Delete(categoria categoria)
        {
            var categoriaEliminar = await _context.categoria.FindAsync(categoria.id);
            if (categoriaEliminar == null)
            {
                return false;
            }
            categoriaEliminar.eliminado = true;
            _context.categoria.Update(categoriaEliminar);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
