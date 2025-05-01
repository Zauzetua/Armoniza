using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Application.Common.Interfaces.Repositories
{
    public interface IApartadosRepository : IRepository<apartado>
    {
        Task Delete(int id);
        public  Task<int> CrearApartado(IEnumerable<int> idsInstrumentos, DateTime fechaRegreso, int idUsuario);
        public  Task<bool> LiberarApartadoAsync(int idApartado);

    }
    
    
}
