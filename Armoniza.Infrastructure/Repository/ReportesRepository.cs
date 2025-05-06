using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Domain.Entities.Vistas;
using Armoniza.Infrastructure.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Armoniza.Infrastructure.Repository
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly ApplicationDbContext _context;
        public ReportesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reporte>> ObtenerRegistros()
        {
            return await _context.Reportes.ToListAsync();
        }
    }
    
    
}
