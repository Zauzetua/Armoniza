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
        private readonly ITipoUsuarioService _tipoUsuarioService;
        private readonly IApartadosRepository _apartadosRepository;


        public UsuarioService(IUsuarioRepository usuarioRepository, ITipoUsuarioService tipoUsuarioService, IApartadosRepository apartadosRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tipoUsuarioService = tipoUsuarioService;
            _apartadosRepository = apartadosRepository;
        }

        public async Task<ServiceResponse<int>> ObtenerMaximoInstrumentos(int id)
        {
            var usuario = _usuarioRepository.Get(u => u.id == id);
            var tipos = _tipoUsuarioService.GetAll(t => t.eliminado == false);
            if (usuario == null)
            {
                return ServiceResponse<int>.Fail("¡Usuario no encontrado!");
            }
            var maximoInstrumentos = tipos.Data.FirstOrDefault(t => t.id == usuario.idTipo).capacidadInstrumentos;

            if (maximoInstrumentos == 0)
            {
                return ServiceResponse<int>.Fail("¡Tipo de usuario no encontrado!");
            }

            return ServiceResponse<int>.Ok(maximoInstrumentos);

        }
        public async Task<ServiceResponse<usuario>> Delete(int id)
        {
            var activo = _apartadosRepository.Any(a => a.idusuario == id && a.activo == true);
            if (activo)
            {
                return ServiceResponse<usuario>.Fail("¡No se puede eliminar el usuario porque tiene apartados activos!");
            }
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
			//Verificar si el correo ya existe
			var usuarioExistente = _usuarioRepository.Get(u => u.correo == usuario.correo);
			if (usuarioExistente != null)
			{
				return ServiceResponse<usuario>.Fail("¡El correo ya está registrado!");
			}
			//Verificar si el telefono ya existe
			var usuarioConTelefonoExistente = _usuarioRepository.Get(u => u.telefono == usuario.telefono);
			if (usuarioConTelefonoExistente != null && usuarioConTelefonoExistente.telefono is not null)
			{
				return ServiceResponse<usuario>.Fail("¡El telefono ya está registrado!");
			}

            if (usuario.telefono is not null)
            {
				//Verificar numero es numerico y de 10 digitos
				if (usuario.telefono.Length != 10 || !usuario.telefono.All(char.IsDigit))
				{
					return ServiceResponse<usuario>.Fail("¡El telefono debe ser un numero de 10 digitos!");
				}

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
			var usuarioConCorreoExistente = _usuarioRepository.Get(u => u.correo == usuario.correo && u.id != usuario.id);
			var usuarioConTelefonoExistente = _usuarioRepository.Get(u => u.telefono == usuario.telefono && u.id != usuario.id);
			if (usuarioConCorreoExistente != null)
			{
				return ServiceResponse<bool>.Fail("¡El correo ya está registrado!");
			}
			if (usuarioConTelefonoExistente != null && usuarioConTelefonoExistente.telefono is not null)
			{
				return ServiceResponse<bool>.Fail("¡El telefono ya está registrado!");
			}
			//Verificar numero es numerico y de 10 digitos
			if (usuario.telefono is not null)
			{
				if (usuario.telefono.Length != 10 || !usuario.telefono.All(char.IsDigit))
				{
					return ServiceResponse<bool>.Fail("¡El telefono debe ser un numero de 10 digitos!");
				}
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
