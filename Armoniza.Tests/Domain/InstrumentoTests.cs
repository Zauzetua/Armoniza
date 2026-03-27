using Armoniza.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Armoniza.Tests.Domain
{
    public class InstrumentoTests
    {

        [Fact]
        public void Prestar_DebeCambiarEstadoAOcupado_SoloSiDisponible()
        {
            // Arrange
            var instrumento = new instrumento
            {
                codigo = 1,
                nombre = "Guitarra Chingona",
                funcional = true,
                eliminado = false,
                ocupado = false,
            };

            // Act
            instrumento.Prestar();

            // Assert
            Assert.True(instrumento.ocupado);
        }

        [Fact]
        public void Prestar_DebeLanzarExcepcion_SiElInstrumentoNoEstaDisponible()
        {
            // Arrange
            var instrumento = new instrumento
            {
                codigo = 2,
                nombre = "Bajo alto",
                funcional = true,
                eliminado = false,
                ocupado = true, // Ya esta ocupado
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => instrumento.Prestar());
        }

        [Fact]
        public void Devolver_DebeCambiarEstadoADisponible_SoloSiOcupado()
        {
            // Arrange
            var instrumento = new instrumento
            {
                codigo = 3,
                nombre = "Bateria Balatro",
                funcional = true,
                eliminado = false,
                ocupado = true, // Esta ocupado
            };

            // Act
            instrumento.Devolver();

            // Assert
            Assert.False(instrumento.ocupado);
        }


    }
}
