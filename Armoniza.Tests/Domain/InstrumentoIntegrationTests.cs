using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;
using Armoniza.Infrastructure.Repository;
using Armoniza.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Armoniza.Tests.Domain
{
    public class InstrumentoIntegrationTests
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=Armoniza_Test;Username=postgres;Password=1001")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private static int GenerarCodigoUnico() => (int)(DateTime.UtcNow.Ticks % int.MaxValue);

        [Fact]
        public async Task CP6_1_DeberiaDeGuardarDb()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var instrumentoRepository = new InstrumentoRepository(context);
            var categoriaRepository = new CategoriasRepository(context);
            var service = new InstrumentoService(instrumentoRepository, categoriaRepository);

            var categoriaPrueba = new categoria
            {
                categoria1 = "Percusion_Test",
                eliminado = false
            };
            context.categoria.Add(categoriaPrueba);
            await context.SaveChangesAsync();

            var codigo = GenerarCodigoUnico();
            var nuevoInstrumento = new instrumento
            {
                codigo = codigo,
                nombre = "Bateria",
                idCategoria = categoriaPrueba.id,
                estuche = false
            };

            // Act
            var resultado = await service.Add(nuevoInstrumento);

            // Assert
            using var assertContext = GetDatabaseContext();
            var registroEnDb = await assertContext.instrumentos.FirstOrDefaultAsync(i => i.codigo == codigo);

            Assert.True(resultado.Success);
            Assert.NotNull(registroEnDb);
            Assert.Equal("Bateria", registroEnDb.nombre);

            // Limpieza
            assertContext.instrumentos.Remove(registroEnDb);
            var categoriaLimpieza = await assertContext.categoria.FirstOrDefaultAsync(c => c.id == categoriaPrueba.id);
            if (categoriaLimpieza is not null)
            {
                assertContext.categoria.Remove(categoriaLimpieza);
            }
            await assertContext.SaveChangesAsync();
        }

        [Fact]
        public async Task CP6_2_ActualizarServiceEnDb()
        {
            // Arrange
            int codigo;
            int categoriaInicialId;
            int categoriaNuevaId;

            using (var setupContext = GetDatabaseContext())
            {
                var categoriaInicial = new categoria { categoria1 = "Viento_Test", eliminado = false };
                var categoriaNueva = new categoria { categoria1 = "Viento_Test_Nueva", eliminado = false };
                setupContext.categoria.Add(categoriaInicial);
                setupContext.categoria.Add(categoriaNueva);
                await setupContext.SaveChangesAsync();

                codigo = GenerarCodigoUnico();
                var instrumentoOriginal = new instrumento
                {
                    codigo = codigo,
                    nombre = "Flauta",
                    idCategoria = categoriaInicial.id,
                    funcional = true,
                    ocupado = false,
                    eliminado = false,
                    estuche = false
                };

                setupContext.instrumentos.Add(instrumentoOriginal);
                await setupContext.SaveChangesAsync();

                categoriaInicialId = categoriaInicial.id;
                categoriaNuevaId = categoriaNueva.id;
            }

            using (var testContext = GetDatabaseContext())
            {
                var instrumentoRepository = new InstrumentoRepository(testContext);
                var categoriaRepository = new CategoriasRepository(testContext);
                var service = new InstrumentoService(instrumentoRepository, categoriaRepository);

                var instrumentoActualizado = new instrumento
                {
                    codigo = codigo,
                    nombre = "Flauta",
                    idCategoria = categoriaNuevaId,
                    estuche = true,
                    funcional = true
                };

                // Act
                var updateResult = service.Update(instrumentoActualizado);

                // Assert
                Assert.True(updateResult.Success);
            }

            using var assertContext = GetDatabaseContext();
            var registroActualizado = await assertContext.instrumentos.FirstOrDefaultAsync(i => i.codigo == codigo);

            Assert.NotNull(registroActualizado);
            Assert.Equal("Flauta", registroActualizado.nombre);
            Assert.Equal(categoriaNuevaId, registroActualizado.idCategoria);

            // Limpieza
            assertContext.instrumentos.Remove(registroActualizado);

            var categoriaInicialLimpieza = await assertContext.categoria.FirstOrDefaultAsync(c => c.id == categoriaInicialId);
            if (categoriaInicialLimpieza is not null)
            {
                assertContext.categoria.Remove(categoriaInicialLimpieza);
            }

            var categoriaNuevaLimpieza = await assertContext.categoria.FirstOrDefaultAsync(c => c.id == categoriaNuevaId);
            if (categoriaNuevaLimpieza is not null)
            {
                assertContext.categoria.Remove(categoriaNuevaLimpieza);
            }

            await assertContext.SaveChangesAsync();
        }

        [Fact]
        public async Task CP6_3_ServiceBorrarDb()
        {
            // Arrange
            int codigo;
            int categoriaId;

            using (var setupContext = GetDatabaseContext())
            {
                var categoriaPrueba = new categoria { categoria1 = "Cuerdas_Test_Delete", eliminado = false };
                setupContext.categoria.Add(categoriaPrueba);
                await setupContext.SaveChangesAsync();

                codigo = GenerarCodigoUnico();
                categoriaId = categoriaPrueba.id;

                var instrumentoAEliminar = new instrumento
                {
                    codigo = codigo,
                    nombre = "Violin",
                    idCategoria = categoriaId,
                    funcional = true,
                    ocupado = false,
                    eliminado = false,
                    estuche = true
                };

                setupContext.instrumentos.Add(instrumentoAEliminar);
                await setupContext.SaveChangesAsync();
            }

            using (var testContext = GetDatabaseContext())
            {
                var instrumentoRepository = new InstrumentoRepository(testContext);
                var categoriaRepository = new CategoriasRepository(testContext);
                var service = new InstrumentoService(instrumentoRepository, categoriaRepository);

                // Act
                var deleteResult = await service.Delete(codigo);

                // Assert
                Assert.True(deleteResult.Success);
            }

            using var assertContext = GetDatabaseContext();
            var registroEliminado = await assertContext.instrumentos.FirstOrDefaultAsync(i => i.codigo == codigo);

            Assert.NotNull(registroEliminado);
            Assert.True(registroEliminado.eliminado);

            // Limpieza (fisica para no contaminar pruebas)
            assertContext.instrumentos.Remove(registroEliminado);
            var categoriaLimpieza = await assertContext.categoria.FirstOrDefaultAsync(c => c.id == categoriaId);
            if (categoriaLimpieza is not null)
            {
                assertContext.categoria.Remove(categoriaLimpieza);
            }
            await assertContext.SaveChangesAsync();
        }
    }
}
