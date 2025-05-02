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
    public class TipoUsuarioRepository : Repository<tipoUsuario>, ITipoUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoUsuarioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> Delete(tipoUsuario tipo)
        {
            var tipoEliminar = _context.tipoUsuarios.Find(tipo.id);
            if (tipoEliminar == null)
            {
                return Task.FromResult(false);
            }
            _context.tipoUsuarios.Update(tipo);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
    
    
}
