using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Domain.Entities.Vistas;

namespace Armoniza.Infrastructure.Services
{
    public class ReportesService : IReportesService
    {
        private readonly IReportesRepository _reportesRepository;
        public ReportesService(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        public async Task<List<Reporte>> ObtenerRegistros()
        {
            var reportes = await _reportesRepository.ObtenerRegistros();

            // Si es nulo, retornar una lista vacía
            if (reportes == null)
            {
                return new List<Reporte>();
            }

            // Agrupar registros por ID, concatenando instrumentos
            var reportesAgrupados = reportes
                .GroupBy(r => r.id)
                .Select(g => new Reporte
                {
                    id = g.Key,
                    usuario = g.First().usuario,
                    grupo = g.First().grupo,
                    fecha_dado = g.First().fecha_dado,
                    fecha_regreso = g.First().fecha_regreso,
                    retornado = g.First().retornado,
                    instrumento = string.Join(", ", g.Select(r => r.instrumento).Distinct())
                })
                .ToList();

            return reportesAgrupados;
        }

    }


}
