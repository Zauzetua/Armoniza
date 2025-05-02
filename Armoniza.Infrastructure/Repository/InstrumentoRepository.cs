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
    public class InstrumentoRepository : Repository<instrumento>, IInstrumentoRepository
    {
        private readonly ApplicationDbContext _context;

        public InstrumentoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<bool> Delete(int id)
        {
            var instrumentoEliminar = _context.instrumentos.Find(id);
            if (instrumentoEliminar == null)
            {
                return Task.FromResult(false);
            }
            instrumentoEliminar.eliminado = true;
            _context.instrumentos.Update(instrumentoEliminar);
            _context.SaveChanges();
            return Task.FromResult(true);

        }

        public Task<IEnumerable<int>> ObtenerCodigos()
        {
            var codigos = _context.instrumentos.Select(x => x.codigo).ToList();
            return Task.FromResult(codigos.AsEnumerable());
        }
    }


}
