using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;

namespace Armoniza.Infrastructure.Repository
{
    public class GrupoRepository : Repository<grupo>, IGrupoRepository

    {
        private readonly ApplicationDbContext _context;
        public GrupoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }


        public Task<bool> Delete(grupo grupo)
        {
            var grupoEliminar = _context.grupos.Find(grupo.id);
            if (grupoEliminar == null)
            {
                return Task.FromResult(false);
            }
            grupo.eliminado = true;
            _context.grupos.Update(grupo);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }


}
