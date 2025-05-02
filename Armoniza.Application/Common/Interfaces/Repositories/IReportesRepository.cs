using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Domain.Entities.Vistas;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public  interface IReportesRepository 
    {
        public  Task<List<Reporte>> ObtenerRegistros();

    }
}
