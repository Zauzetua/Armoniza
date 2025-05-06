using Armoniza.Application.Common.Models;
using Armoniza.Domain.Entities;
using Armoniza.Domain.Entities.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Services
{
    public interface IApartadosService
    {
        Task<ServiceResponse<apartado>> LiberarApartado(int id);
        Task<ServiceResponse<apartado>> CrearApartado(ApartadoViewModel apartado);
        ServiceResponse<IEnumerable<apartado>> GetAll(Expression<Func<apartado, bool>> filter, string? includeProperties = null);
        ServiceResponse<IEnumerable<apartado>> GetAll();
        ServiceResponse<apartado> Get(Expression<Func<apartado, bool>> filter, string? includeProperties = null);
        
        ServiceResponse<bool> Any(Expression<Func<apartado, bool>> filter, string? includePropierties = null);

        public ServiceResponse<List<InstrumentoUsuario>> GetInstrumentosPorUsuario(int id);



	}
}
