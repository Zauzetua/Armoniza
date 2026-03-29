using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Interfaces.Services;
using Armoniza.Application.Common.Models;
using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Services;
using Moq;
using Xunit;

namespace Armoniza.Tests.Services
{
    public class PrestamoTests
    {
        // la mockeada chat 
        private readonly Mock<IApartadosRepository> _apartadosRepoMock;
        private readonly Mock<IInstrumentoRepository> _instrumentoRepoMock;
        private readonly Mock<IUsuarioService> _usuarioServiceMock;
        private readonly ApartadosService _sut;

        public PrestamoTests()
        {
            _apartadosRepoMock = new Mock<IApartadosRepository>();
            _instrumentoRepoMock = new Mock<IInstrumentoRepository>();
            _usuarioServiceMock = new Mock<IUsuarioService>();
            _sut = new ApartadosService(_apartadosRepoMock.Object, _instrumentoRepoMock.Object, _usuarioServiceMock.Object);
        }

        [Fact]
        public async Task CrearApartado_DebeCrearApartado_CuandoDatosSonValidos()
        {
            // Arrange
            var usuarioId = 10;
            var codigoInstrumento = 1;

            var viewModel = new ApartadoViewModel
            {
                apartado = new apartado
                {
                    idusuario = usuarioId,
                    fecharegreso = DateTime.Now.AddDays(5)
                },
                instrumentosSeleccionados = new List<int> { codigoInstrumento }
            };

            _usuarioServiceMock.Setup(s => s.Get(It.IsAny<System.Linq.Expressions.Expression<Func<usuario, bool>>>()))
                .Returns(new ServiceResponse<usuario>
                {
                    Data = new usuario { id = usuarioId, eliminado = false },
                    Success = true,
                    Message = string.Empty
                });

            _usuarioServiceMock.Setup(s => s.ObtenerMaximoInstrumentos(usuarioId))
                .ReturnsAsync(new ServiceResponse<int>
                {
                    Data = 3,
                    Success = true,
                    Message = string.Empty
                });

            _apartadosRepoMock.Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<apartado, bool>>>(), It.IsAny<string?>()))
                .Returns((apartado)null);

            _instrumentoRepoMock.Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<instrumento, bool>>>(), It.IsAny<string?>()))
                .Returns(new instrumento
                {
                    codigo = codigoInstrumento,
                    nombre = "Guitarra",
                    eliminado = false,
                    ocupado = false,
                    funcional = true
                });

            _apartadosRepoMock.Setup(r => r.CrearApartado(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), usuarioId))
                .ReturnsAsync(100);

            // Act
            var result = await _sut.CrearApartado(viewModel);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(100, result.Data.id);
            _apartadosRepoMock.Verify(r => r.CrearApartado(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), usuarioId), Times.Once);
        }

        [Fact]
        public async Task CrearApartado_DebeFallar_CuandoUsuarioNoExiste()
        {
            // Arrange
            var viewModel = new ApartadoViewModel
            {
                apartado = new apartado
                {
                    idusuario = 99,
                    fecharegreso = DateTime.Now.AddDays(2)
                },
                instrumentosSeleccionados = new List<int> { 1 }
            };

            _usuarioServiceMock.Setup(s => s.Get(It.IsAny<System.Linq.Expressions.Expression<Func<usuario, bool>>>()))
                .Returns(new ServiceResponse<usuario>
                {
                    Data = null,
                    Success = false,
                    Message = "No se encontro el usuario"
                });

            // Act
            var result = await _sut.CrearApartado(viewModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("El usuario no existe", result.Message);
            _apartadosRepoMock.Verify(r => r.CrearApartado(It.IsAny<IEnumerable<int>>(), It.IsAny<DateTime>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task CrearApartado_DebeFallarValidacion_CuandoNoHayInstrumentosSeleccionados()
        {
            // Arrange
            var viewModel = new ApartadoViewModel
            {
                apartado = new apartado
                {
                    idusuario = 10,
                    fecharegreso = DateTime.Now.AddDays(2)
                },
                instrumentosSeleccionados = new List<int>()
            };

            // Act
            var result = await _sut.CrearApartado(viewModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("No se han seleccionado instrumentos", result.Message);
            _usuarioServiceMock.Verify(s => s.Get(It.IsAny<System.Linq.Expressions.Expression<Func<usuario, bool>>>()), Times.Never);
        }
    }
}
