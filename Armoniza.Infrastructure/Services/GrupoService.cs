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
        private readonly IUsuarioRepository _usuarioRepository;


        public GrupoService(IGrupoRepository grupoRepository, IUsuarioRepository usuarioRepository)
        {
            _grupoRepository = grupoRepository;
            _usuarioRepository = usuarioRepository;
        }
        public async Task<ServiceResponse<grupo>> Delete(int id)
        {
            var grupo = _grupoRepository.Get(g => g.id == id);

            if (grupo == null) return ServiceResponse<grupo>.Fail("El grupo no existe");

            //Actualizar cuando tenga el servicio de usuarios
            var hayUsuarios = _usuarioRepository.Any(u => u.idGrupo == grupo.id && u.eliminado == false);
            if (hayUsuarios)
            {
               return ServiceResponse<grupo>.Fail("No se puede eliminar el grupo porque tiene usuarios asignados");
            }
            grupo.eliminado = true;
            var grupoEliminar = await _grupoRepository.Delete(grupo);
            if (!grupoEliminar) return ServiceResponse<grupo>.Fail("No se pudo eliminar el grupo");
            return ServiceResponse<grupo>.Ok(grupo);
        }

        public  ServiceResponse<grupo> Add(grupo grupo)
        {
            if (grupo == null)
            {
                return ServiceResponse<grupo>.Fail("El grupo esta vacio");

            }

            //Actualizar cuando tenga el servicio de usuarios
            grupo.eliminado = false;
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

        public ServiceResponse<grupo> Get(Expression<Func<grupo, bool>> filter)
        {
            var grupo = _grupoRepository.Get(filter);
            if (grupo == null) return ServiceResponse<grupo>.Fail("No se encontro el grupo");
            return ServiceResponse<grupo>.Ok(grupo);
        }

        public  ServiceResponse<bool> Update(grupo grupo)
        {
            var grupoExistente = _grupoRepository.Get(g => g.id == grupo.id);
            if (grupoExistente == null) return ServiceResponse<bool>.Fail("El grupo no existe");
            //Actualizar cuando tenga el servicio de usuarios
            grupoExistente.grupo1 = grupo.grupo1;
            var grupoActualizar = _grupoRepository.Update(grupoExistente);
            if (grupoActualizar == false) return ServiceResponse<bool>.Fail("No se pudo actualizar el grupo");
            return ServiceResponse<bool>.Ok(grupoActualizar, "Grupo actualizado de forma exitosa");
        }

        public ServiceResponse<bool> Any(Expression<Func<grupo, bool>> filter, string? includePropierties = null)
        {
            var grupo =  _grupoRepository.Any(filter, includePropierties);
            if (grupo == false) return ServiceResponse<bool>.Fail("No se encontro el grupo");
            return ServiceResponse<bool>.Ok(true);
        }
    }
}