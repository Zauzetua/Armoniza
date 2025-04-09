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
    public class UsuarioRepository : Repository<usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Delete(usuario usuario)
        {

            var usuarioEliminar = await _context.usuarios.FindAsync(usuario.id);
            if (usuarioEliminar == null)
            {
                return false;
            }
            _context.usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
