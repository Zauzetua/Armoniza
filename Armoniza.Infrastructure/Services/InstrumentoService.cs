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
using Armoniza.Infrastructure.Repository;

namespace Armoniza.Infrastructure.Services
{
    public class InstrumentoService : IInstrumentoService
    {
        private readonly IInstrumentoRepository _instrumentoRepository;
        public InstrumentoService(IInstrumentoRepository instrumentoRepository)
        {
            _instrumentoRepository = instrumentoRepository;
        }

        public async Task<ServiceResponse<instrumento>> Add(instrumento instrumento)
        {
            if (instrumento == null)
            {
                return new ServiceResponse<instrumento>
                {
                    Success = false,
                    Message = "El instrumento no puede ser nulo"
                };
            }


            if (instrumento.idCategoria == 0)
            {
                return new ServiceResponse<instrumento>
                {
                    Success = false,
                    Message = "Categoria invalida"
                };

            }

            if (instrumento.codigo <=0)
            {
                return new ServiceResponse<instrumento>
                {
                    Success = false,
                    Message = "El codigo no es valido"
                };
                
            }
            var Existe =  _instrumentoRepository.Any(x => x.codigo == instrumento.codigo);
            if (Existe)
            {
                return new ServiceResponse<instrumento>
                {
                    Success = false,
                    Message = "El codigo ya existe"
                };
            }



            instrumento.eliminado = false;
            instrumento.funcional = true;
            instrumento.ocupado = false;

            var resultado = _instrumentoRepository.Add(instrumento);
            if (resultado == false)
            {
                return new ServiceResponse<instrumento>
                {
                    Success = false,
                    Message = "Error al agregar el instrumento"
                };
            }
            return new ServiceResponse<instrumento>
            {
                Success = true,
                Message = "Instrumento agregado correctamente",
                Data = instrumento
            };


        }

        public ServiceResponse<bool> Any(Expression<Func<instrumento, bool>> filter, string? includePropierties = null)
        {
            var instrumento = _instrumentoRepository.Any(filter, includePropierties);
            if (instrumento == false) return ServiceResponse<bool>.Fail("No se encontro el instrumento");
            return ServiceResponse<bool>.Ok(true);
        }

        public ServiceResponse<bool> Arreglado(int codigo)
        {
            var instrumento = _instrumentoRepository.Get(i => i.codigo == codigo);
            if (instrumento == null) return ServiceResponse<bool>.Fail("El instrumento no existe");
            if (instrumento.funcional == true) return ServiceResponse<bool>.Fail("El instrumento ya esta funcional");
            instrumento.funcional = true;
            var resultado = _instrumentoRepository.Update(instrumento);
            if (resultado == false) return ServiceResponse<bool>.Fail("No se pudo actualizar el instrumento");
            return ServiceResponse<bool>.Ok(true, "Instrumento actualizado correctamente");

        }

        public async Task<ServiceResponse<bool>> Delete(int codigo)
        {
            //Actualizar cuando tenga el servicio de apartados
            var instrumento = _instrumentoRepository.Get(i => i.codigo == codigo);
            if (instrumento == null) return ServiceResponse<bool>.Fail("El instrumento no existe");
            instrumento.eliminado = true;
            var resultado = await _instrumentoRepository.Delete(instrumento.codigo);
            if (resultado == false) return ServiceResponse<bool>.Fail("No se pudo eliminar el instrumento");
            return ServiceResponse<bool>.Ok(true, "Instrumento eliminado correctamente");

        }

        public ServiceResponse<bool> Desocupar(int codigo)
        {
            //Actualizar cuando tenga el servicio de apartados
            var instrumento = _instrumentoRepository.Get(i => i.codigo == codigo);
            if (instrumento == null) return ServiceResponse<bool>.Fail("El instrumento no existe");
            if (instrumento.ocupado == false) return ServiceResponse<bool>.Fail("El instrumento ya esta desocupado");
            instrumento.ocupado = false;
            var resultado = _instrumentoRepository.Update(instrumento);
            if (resultado == false) return ServiceResponse<bool>.Fail("No se pudo actualizar el instrumento");
            return ServiceResponse<bool>.Ok(true, "Instrumento desocupado correctamente");
        }

        public ServiceResponse<instrumento> Get(Expression<Func<instrumento, bool>> filter)
        {
            var instrumento = _instrumentoRepository.Get(filter);
            if (instrumento == null) return ServiceResponse<instrumento>.Fail("No se encontro el instrumento");
            return ServiceResponse<instrumento>.Ok(instrumento);
        }

        public ServiceResponse<IEnumerable<instrumento>> GetAll(Expression<Func<instrumento, bool>> filter)
        {
            var instrumentos = _instrumentoRepository.GetAll(filter);
            if (instrumentos == null) return ServiceResponse<IEnumerable<instrumento>>.Fail("No se encontraron instrumentos");
            return ServiceResponse<IEnumerable<instrumento>>.Ok(instrumentos);
        }

        public ServiceResponse<IEnumerable<instrumento>> GetAll()
        {
            var instrumentos = _instrumentoRepository.GetAll();
            if (instrumentos == null) return ServiceResponse<IEnumerable<instrumento>>.Fail("No se encontraron instrumentos");
            return ServiceResponse<IEnumerable<instrumento>>.Ok(instrumentos);
        }

        public ServiceResponse<bool> Ocupar(int codigo)
        {
            var instrumento = _instrumentoRepository.Get(i => i.codigo == codigo);
            if (instrumento == null) return ServiceResponse<bool>.Fail("El instrumento no existe");
            if (instrumento.ocupado == true) return ServiceResponse<bool>.Fail("El instrumento ya esta ocupado");
            instrumento.ocupado = true;
            var resultado = _instrumentoRepository.Update(instrumento);
            if (resultado == false) return ServiceResponse<bool>.Fail("No se pudo actualizar el instrumento");
            return ServiceResponse<bool>.Ok(true, "Instrumento apartado correctamente");
        }

        public ServiceResponse<bool> Roto(int codigo)
        {
            var instrumento = _instrumentoRepository.Get(i => i.codigo == codigo);
            if (instrumento == null) return ServiceResponse<bool>.Fail("El instrumento no existe");
            if (instrumento.funcional == false) return ServiceResponse<bool>.Fail("El instrumento ya esta roto");
            instrumento.funcional = false;
            var resultado = _instrumentoRepository.Update(instrumento);
            if (resultado == false) return ServiceResponse<bool>.Fail("No se pudo actualizar el instrumento");
            return ServiceResponse<bool>.Ok(true, "Instrumento actualizado correctamente");

        }

        public ServiceResponse<bool> Update(instrumento instrumento)
        {
            var instrumentoExistente = _instrumentoRepository.Get(i => i.codigo == instrumento.codigo);
            if (instrumentoExistente == null) return ServiceResponse<bool>.Fail("El instrumento no existe");

            if (instrumento.idCategoria <= 0)
            {
                return ServiceResponse<bool>.Fail("Categoria invalida");
            }

            if (instrumento.codigo <= 0)
            {
                return ServiceResponse<bool>.Fail("El codigo no es valido");
            }

            var existe = _instrumentoRepository.Any(x => x.codigo == instrumento.codigo && x.codigo != instrumentoExistente.codigo);
            if (existe)
            {
                return ServiceResponse<bool>.Fail("El codigo ya existe");
            }
            //Solo estas propiedades, las demas tienen su propio metodo, ya que involucran mas logica
            instrumentoExistente.codigo = instrumento.codigo;
            instrumentoExistente.nombre = instrumento.nombre;
            instrumentoExistente.estuche = instrumento.estuche;
            instrumentoExistente.idCategoria = instrumento.idCategoria;
            var resultado = _instrumentoRepository.Update(instrumentoExistente);
            if (resultado == false) return ServiceResponse<bool>.Fail("No se pudo actualizar el instrumento");
            return ServiceResponse<bool>.Ok(true, "Instrumento actualizado correctamente");
        }

    }
    
    
}
