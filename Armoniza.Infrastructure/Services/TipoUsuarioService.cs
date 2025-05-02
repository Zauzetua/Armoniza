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
    public class TipoUsuarioService : ITipoUsuarioService
    {

        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public TipoUsuarioService(ITipoUsuarioRepository tipoUsuarioRepository, IUsuarioRepository usuarioRepository)
        {
            _tipoUsuarioRepository = tipoUsuarioRepository;
            _usuarioRepository = usuarioRepository;
        }

        public Task<ServiceResponse<bool>> Add(tipoUsuario instrumento)
        {
            var tipo = _tipoUsuarioRepository.Get(t => t.tipo == instrumento.tipo);
            if (tipo != null)
            {
                return Task.FromResult(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Tipo de usuario ya existe"
                });
            }
            var result = _tipoUsuarioRepository.Add(instrumento);
            if (result == false)
            {
                return Task.FromResult(new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Error al agregar el tipo de usuario"
                });
            }
            return Task.FromResult(new ServiceResponse<bool>
            {
                Success = true,
                Message = "Tipo de usuario agregado correctamente"
            });
        }

        public ServiceResponse<bool> Any(Expression<Func<tipoUsuario, bool>> filter, string? includePropierties = null)
        {
            var result = _tipoUsuarioRepository.Any(filter, includePropierties);
            return new ServiceResponse<bool>
            {
                Data = result,
                Success = true
            };

        }

        public ServiceResponse<bool> Delete(int id)
        {
            var tipo = _tipoUsuarioRepository.Get(t => t.id == id);
            var enuso = _usuarioRepository.Any(u => u.idTipo == id);
            if (enuso)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "No se puede eliminar el tipo de usuario porque está en uso"
                };
            }
            if (tipo == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Tipo de usuario no encontrado"
                };
            }
            var result = _tipoUsuarioRepository.Delete(tipo);
            if (result == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Error al eliminar el tipo de usuario"
                };
            }
            return new ServiceResponse<bool>
            {
                Success = true,
                Message = "Tipo de usuario eliminado correctamente"
            };

        }

        public ServiceResponse<tipoUsuario> Get(Expression<Func<tipoUsuario, bool>> filter)
        {
            var tipo = _tipoUsuarioRepository.Get(filter);
            if (tipo == null)
            {
                return new ServiceResponse<tipoUsuario>
                {
                    Success = false,
                    Message = "Tipo de usuario no encontrado"
                };
            }
            return new ServiceResponse<tipoUsuario>
            {
                Success = true,
                Data = tipo
            };


        }

        public ServiceResponse<IEnumerable<tipoUsuario>> GetAll(Expression<Func<tipoUsuario, bool>> filter)
        {
            var tipos = _tipoUsuarioRepository.GetAll(filter);
            if (tipos == null)
            {
                return new ServiceResponse<IEnumerable<tipoUsuario>>
                {
                    Success = false,
                    Message = "No se encontraron tipos de usuario"
                };
            }
            return new ServiceResponse<IEnumerable<tipoUsuario>>
            {
                Success = true,
                Data = tipos
            };

        }

        public ServiceResponse<IEnumerable<tipoUsuario>> GetAll()
        {
            var tipos = _tipoUsuarioRepository.GetAll();
            if (tipos == null)
            {
                return new ServiceResponse<IEnumerable<tipoUsuario>>
                {
                    Success = false,
                    Message = "No se encontraron tipos de usuario"
                };
            }
            return new ServiceResponse<IEnumerable<tipoUsuario>>
            {
                Success = true,
                Data = tipos
            };

        }

        public ServiceResponse<bool> Update(tipoUsuario instrumento)
        {
            var tipo = _tipoUsuarioRepository.Get(t => t.id == instrumento.id);
            if (tipo == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Tipo de usuario no encontrado"
                };
            }
            tipo.tipo = instrumento.tipo;
            var result = _tipoUsuarioRepository.Update(tipo);
            if (result == false)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Error al actualizar el tipo de usuario"
                };
            }
            return new ServiceResponse<bool>
            {
                Success = true,
                Message = "Tipo de usuario actualizado correctamente"
            };

        }
    }


}
