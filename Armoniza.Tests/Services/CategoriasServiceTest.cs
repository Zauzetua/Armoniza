using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Armoniza.Tests.Services
{
    public class CategoriasServiceTest
    {
        private readonly Mock<ICategoriasRepository> _categoriasRepositoryMock;
        private readonly CategoriasService _categoriasService;

        public CategoriasServiceTest()
        {
            _categoriasRepositoryMock = new Mock<ICategoriasRepository>();
            _categoriasService = new CategoriasService(_categoriasRepositoryMock.Object);
        }

        [Fact]
        public async Task Add_ShouldReturnTrue_WhenCategoriaIsValid()
        {
            // Arrange
            var categoria = new categoria { id = 1, categoria1 = "Categoria 1", eliminado = false };

            _categoriasRepositoryMock.Setup(r => r.Add(It.IsAny<categoria>()));
            _categoriasRepositoryMock.Setup(r => r.save());

            // Act
            var result = await _categoriasService.Add(categoria);

            // Assert
            Assert.True(result);
            _categoriasRepositoryMock.Verify(r => r.Add(It.Is<categoria>(c => c == categoria)), Times.Once);
            _categoriasRepositoryMock.Verify(r => r.save(), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnFalse_WhenCategoriaIsNull()
        {
            // Act
            var result = await _categoriasService.Add(null);

            // Assert
            Assert.False(result);
            _categoriasRepositoryMock.Verify(r => r.Add(It.IsAny<categoria>()), Times.Never);
            _categoriasRepositoryMock.Verify(r => r.save(), Times.Never);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenCategoriaExists()
        {
            // Arrange
            var categoria = new categoria { id = 1, categoria1 = "Categoria 1", eliminado = false };

            _categoriasRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<categoria, bool>>>())).Returns(categoria);
            _categoriasRepositoryMock.Setup(r => r.Update(It.IsAny<categoria>()));
            _categoriasRepositoryMock.Setup(r => r.save());

            // Act
            var result = await _categoriasService.Delete(categoria.id);

            // Assert
            Assert.True(result);
            _categoriasRepositoryMock.Verify(r => r.Update(It.Is<categoria>(c => c.eliminado == true)), Times.Once);
            _categoriasRepositoryMock.Verify(r => r.save(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenCategoriaDoesNotExist()
        {
            // Arrange
            _categoriasRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<categoria, bool>>>())).Returns((categoria)null);

            // Act
            var result = await _categoriasService.Delete(1);

            // Assert
            Assert.False(result);
            _categoriasRepositoryMock.Verify(r => r.Update(It.IsAny<categoria>()), Times.Never);
            _categoriasRepositoryMock.Verify(r => r.save(), Times.Never);
        }

        [Fact]
        public async Task GetAll_ShouldReturnCategorias()
        {
            // Arrange
            var categorias = new List<categoria>
            {
                new categoria { id = 1, categoria1 = "Categoria 1" },
                new categoria { id = 2, categoria1 = "Categoria 2" }
            }.AsEnumerable();

            _categoriasRepositoryMock.Setup(r => r.GetAll()).Returns(categorias);

            // Act
            var result = await _categoriasService.GetAll();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.categoria1 == "Categoria 1");
            Assert.Contains(result, c => c.categoria1 == "Categoria 2");
        }

        [Fact]
        public async Task Update_ShouldReturnTrue_WhenCategoriaIsValid()
        {
            // Arrange
            var categoria = new categoria { id = 1, categoria1  = "Categoria Actualizada" };

            _categoriasRepositoryMock.Setup(r => r.Update(It.IsAny<categoria>()));
            _categoriasRepositoryMock.Setup(r => r.save());

            // Act
            var result = await _categoriasService.Update(categoria);

            // Assert
            Assert.True(result);
            _categoriasRepositoryMock.Verify(r => r.Update(It.Is<categoria>(c => c == categoria)), Times.Once);
            _categoriasRepositoryMock.Verify(r => r.save(), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnFalse_WhenCategoriaIsNull()
        {
            // Act
            var result = await _categoriasService.Update(null);

            // Assert
            Assert.False(result);
            _categoriasRepositoryMock.Verify(r => r.Update(It.IsAny<categoria>()), Times.Never);
            _categoriasRepositoryMock.Verify(r => r.save(), Times.Never);
        }
    }
}