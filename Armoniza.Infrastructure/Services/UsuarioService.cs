using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Application.Common.Models;
using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Infrastructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;


        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
           
        }

        public async Task<ServiceResponse<usuario>> Delete(int id)
        {
            var usuario = _usuarioRepository.Get(u => u.id == id);
            if (usuario == null)
            {
                return ServiceResponse<usuario>.Fail("¡Usuario no encontrado!");
            }
            usuario.eliminado = true;
            await _usuarioRepository.Delete(usuario);
            return ServiceResponse<usuario>.Ok(usuario);

        }

        public ServiceResponse<usuario> Add(usuario usuario)
        {
            if (usuario == null)
            {
                return ServiceResponse<usuario>.Fail("¡Usuario no encontrado!");
            }

            
            usuario.eliminado = false;
            _usuarioRepository.Add(usuario);
            return ServiceResponse<usuario>.Ok(usuario, "¡Usuario agregado exitosamente!");

        }

        public ServiceResponse<IEnumerable<usuario>> GetAll(Expression<Func<usuario, bool>> filter)
        {
            var usuarios = _usuarioRepository.GetAll(filter);
            if (usuarios == null)
            {
                return ServiceResponse<IEnumerable<usuario>>.Fail("¡No se encontraron usuarios!");
            }
            usuarios = usuarios.OrderBy(u => u.nombreCompleto);
            return ServiceResponse<IEnumerable<usuario>>.Ok(usuarios);
        }

        public ServiceResponse<IEnumerable<usuario>> GetAll()
        {
            var usuarios = _usuarioRepository.GetAll();
            if (usuarios == null)
            {
                return ServiceResponse<IEnumerable<usuario>>.Fail("¡No se encontraron usuarios!");
            }
            return ServiceResponse<IEnumerable<usuario>>.Ok(usuarios);
        }

        public ServiceResponse<usuario> Get(Expression<Func<usuario, bool>> filter)
        {
            var usuario = _usuarioRepository.Get(filter);
            if (usuario == null)
            {
                return ServiceResponse<usuario>.Fail("¡No se encontró el usuario!");
            }
            return ServiceResponse<usuario>.Ok(usuario);
        }

        public ServiceResponse<bool> Update(usuario usuario)
        {
            var usuarioExistente = _usuarioRepository.Get(u => u.id == usuario.id);
            if (usuarioExistente == null)
            {
                return ServiceResponse<bool>.Fail("¡Usuario no encontrado!");
            }
            //Actualizar cuando tenga el servicio de usuarios
            usuarioExistente.correo = usuario.correo;
            usuarioExistente.telefono = usuario.telefono;
            usuarioExistente.nombreCompleto = usuario.nombreCompleto;
            usuarioExistente.idGrupo = usuario.idGrupo;
            usuarioExistente.idTipo = usuario.idTipo;

            _usuarioRepository.Update(usuarioExistente);
            return ServiceResponse<bool>.Ok(true, "¡Usuario actualizado exitosamente!");
        }

        public ServiceResponse<bool> Any(Expression<Func<usuario, bool>> filter, string? includePropierties = null)
        {
            var usuario = _usuarioRepository.Any(filter, includePropierties);
            if (usuario == false)
            {
                return ServiceResponse<bool>.Fail("¡No se encontró el usuario!");
            }
            return ServiceResponse<bool>.Ok(true);
        }


    }
}
