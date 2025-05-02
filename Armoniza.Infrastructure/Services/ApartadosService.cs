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
    public class ApartadosService : IApartadosService
    {
        private readonly IApartadosRepository _apartadosRepository;
        private IInstrumentoRepository _instrumentoRepository;
        private readonly IUsuarioService _usuarioService;

        public ApartadosService(IApartadosRepository apartadosRepository, IInstrumentoRepository instrumentoRepository, IUsuarioService usuarioService)
        {
            _apartadosRepository = apartadosRepository;
            _instrumentoRepository = instrumentoRepository;
            _usuarioService = usuarioService;
        }

        public ServiceResponse<bool> Any(Expression<Func<apartado, bool>> filter, string? includePropierties = null)
        {
            var result = _apartadosRepository.Any(filter, includePropierties);
            return new ServiceResponse<bool>
            {
                Data = result,
                Success = true
            };
        }
        //Usar el proc almacenado
        public async Task<ServiceResponse<apartado>> CrearApartado(ApartadoViewModel apartado)
        {
            if (apartado is null)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "El apartado no puede estar vacio"
                };

            }
            if (apartado.instrumentosSeleccionados.Count() == 0)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "No se han seleccionado instrumentos"
                };
            }
            //Verificar que no se repitan los instrumentos
            var instrumentosRepetidos = apartado.instrumentosSeleccionados.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key);
            if (instrumentosRepetidos.Any())
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "Los instrumentos no pueden estar repetidos"
                };
            }
            //Verificar que no exceda su limite de instrumentos
            var usuario = _usuarioService.Get(x => x.id == apartado.apartado.idusuario);
            if (usuario.Data is null)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "El usuario no existe"
                };
            }
            if (usuario.Data.eliminado == true)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "El usuario no existe"
                };
            }
            var maximoInstrumentos = await _usuarioService.ObtenerMaximoInstrumentos(usuario.Data.id);
            if (maximoInstrumentos.Data == 0)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "Usuario no valido"
                };
            }
            if (apartado.instrumentosSeleccionados.Count() > maximoInstrumentos.Data)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "El usuario no puede apartar tantos instrumentos"
                };
            }

            //verificar que la fecha de regreso no sea menor a la fecha actual
            if (apartado.apartado.fecharegreso < DateTime.Now)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "La fecha de regreso no puede ser menor a la fecha actual"
                };
            }

            //verificar que el usuario no tenga un apartado activo
            var apartadoActivo = _apartadosRepository.Get(x => x.idusuario == apartado.apartado.idusuario && x.activo == true);
            if (apartadoActivo != null)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "El usuario ya tiene un apartado activo"
                };
            }

            //Verificar que los instrumentos no esten ocupados
            foreach (var item in apartado.instrumentosSeleccionados)
            {
                var instrumento = _instrumentoRepository.Get(x => x.codigo == item);
                if (instrumento is null)
                {
                    return new ServiceResponse<apartado>
                    {
                        Data = null,
                        Success = false,
                        Message = "Un instrumento no existe"
                    };
                }
                if (instrumento.eliminado == true)
                {
                    return new ServiceResponse<apartado>
                    {
                        Data = null,
                        Success = false,
                        Message = "El instrumento no existe"
                    };
                }
                if (instrumento.ocupado == true)
                {
                    return new ServiceResponse<apartado>
                    {
                        Data = null,
                        Success = false,
                        Message = "El instrumento ya esta ocupado"
                    };
                }
            }
            //Crear el apartado
            var instrumentosId = apartado.instrumentosSeleccionados.ToList();
            var resultado = await _apartadosRepository.CrearApartado(instrumentosId, apartado.apartado.fecharegreso, apartado.apartado.idusuario);
            if (resultado == 0)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "Error al crear el apartado"
                };
            }
            return new ServiceResponse<apartado>
            {
                Data = new apartado
                {
                    id = resultado,
                    fechadado = DateTime.Now,
                    fecharegreso = apartado.apartado.fecharegreso,
                    activo = true,
                    idusuario = apartado.apartado.idusuario,
                },
                Success = true,
                Message = "Apartado creado con exito"
            };


        }

        public ServiceResponse<apartado> Get(Expression<Func<apartado, bool>> filter, string? includeProperties = null)
        {
            var result = _apartadosRepository.Get(filter, includeProperties);
            if (result == null)
            {
                return new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "No se encontro el apartado"
                };
            }
            return new ServiceResponse<apartado>
            {
                Data = result,
                Success = true
            };

        }

        public ServiceResponse<IEnumerable<apartado>> GetAll(Expression<Func<apartado, bool>> filter, string? includeProperties = null)
        {
            var result = _apartadosRepository.GetAll(filter, includeProperties);
            if (result == null)
            {
                return new ServiceResponse<IEnumerable<apartado>>
                {
                    Data = null,
                    Success = false,
                    Message = "No se encontraron apartados"
                };
            }
            return new ServiceResponse<IEnumerable<apartado>>
            {
                Data = result,
                Success = true
            };

        }

        public ServiceResponse<IEnumerable<apartado>> GetAll()
        {
            var result = _apartadosRepository.GetAll();
            if (result == null)
            {
                return new ServiceResponse<IEnumerable<apartado>>
                {
                    Data = null,
                    Success = false,
                    Message = "No se encontraron apartados"
                };
            }
            return new ServiceResponse<IEnumerable<apartado>>
            {
                Data = result,
                Success = true
            };
        }

        public Task<ServiceResponse<apartado>> LiberarApartado(int id)
        {
            var result = _apartadosRepository.LiberarApartadoAsync(id);
            if (result.Result == false)
            {
                return Task.FromResult(new ServiceResponse<apartado>
                {
                    Data = null,
                    Success = false,
                    Message = "No se pudo liberar el apartado"
                });
            }
            return Task.FromResult(new ServiceResponse<apartado>
            {
                Data = null,
                Success = true,
                Message = "Apartado liberado con exito"
            });
        }
    }


}
