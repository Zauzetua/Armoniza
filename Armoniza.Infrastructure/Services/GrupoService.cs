using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Application.Common.Models;
using Armoniza.Domain.Entities;

namespace Armoniza.Infrastructure.Services
{
    public class GrupoService : IGrupoService
    {
        private readonly IGrupoRepository _grupoRepository;


        public GrupoService(IGrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;

        }
        public async Task<ServiceResponse<grupo>> Delete(int id)
        {
            var grupo = _grupoRepository.Get(g => g.id == id);

            if (grupo == null) return ServiceResponse<grupo>.Fail("El grupo no existe");

            //Actualizar cuando tenga el servicio de usuarios
            var grupoEliminar = await _grupoRepository.Delete(grupo);
            if (!grupoEliminar) return ServiceResponse<grupo>.Fail("No se pudo eliminar el grupo");
            return ServiceResponse<grupo>.Ok(grupo);
        }

        public  ServiceResponse<grupo> Add(grupo grupo)
        {
            var grupoExistente = _grupoRepository.Get(g => g.grupo1 == grupo.grupo1);
            if (grupoExistente == null)
                return ServiceResponse<grupo>.Fail("El grupo ya existe");
            //Actualizar cuando tenga el servicio de usuarios

             _grupoRepository.Add(grupo);
             return ServiceResponse<grupo>.Ok(grupo, "Grupo agregado de forma exitoss!");

        }

        public  ServiceResponse<IEnumerable<grupo>> GetAll(Expression<Func<grupo, bool>> filter)
        {
            var grupos =  _grupoRepository.GetAll(filter);
            if (grupos == null) return ServiceResponse<IEnumerable<grupo>>.Fail("No se encontraron grupos");
            return ServiceResponse<IEnumerable<grupo>>.Ok(grupos);
        }

        public  ServiceResponse<IEnumerable<grupo>> GetAll()
        {
            var grupos =  _grupoRepository.GetAll();
            if (grupos == null) return ServiceResponse<IEnumerable<grupo>>.Fail("No se encontraron grupos");
            return ServiceResponse<IEnumerable<grupo>>.Ok(grupos);
        }
    }
}