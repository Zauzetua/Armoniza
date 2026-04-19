using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;
using Armoniza.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Armoniza.Tests.Domain
{
    public class InstrumentoRepositoryTests
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

        [Fact]
        public async Task CP3_1_DeberiaDePersistirInstrumentoEnPostgreSQL()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var repository = new InstrumentoRepository(context);
            var categoriaPrueba = new categoria
            {
                categoria1 = "Cuerdas_Test",
                eliminado = false
            };
            context.categoria.Add(categoriaPrueba);
            await context.SaveChangesAsync();

            var codigo = (int)(DateTime.UtcNow.Ticks % int.MaxValue);

            var nuevoInstrumento = new instrumento
            {
                codigo = codigo,
                nombre = "Guitarra Acustica",
                idCategoria = categoriaPrueba.id,
                funcional = true,
                ocupado = false,
                eliminado = false,
                estuche = true
            };

            // Act (Paso 2: Llamar al metodo Add)
            var agregado = repository.Add(nuevoInstrumento);

            // Assert (Paso 3: Consultar la BD para verificar)
            var instrumentoEnDb = await context.instrumentos.FirstOrDefaultAsync(i => i.codigo == codigo);

            Assert.True(agregado);
            Assert.NotNull(instrumentoEnDb);
            Assert.Equal("Guitarra Acustica", instrumentoEnDb.nombre);
            Assert.Equal(categoriaPrueba.id, instrumentoEnDb.idCategoria);

            // Limpieza
            context.instrumentos.Remove(instrumentoEnDb);
            context.categoria.Remove(categoriaPrueba);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task CP3_2_DeberiaDeTraerInstrumentoCorrecto()
        {
            // Arrange (Paso 1: Insertar registro directo en BD)
            using var setupContext = GetDatabaseContext();
            var categoriaPrueba = new categoria
            {
                categoria1 = "Cuerdas_Test_2",
                eliminado = false
            };
            setupContext.categoria.Add(categoriaPrueba);
            await setupContext.SaveChangesAsync();

            var codigo = (int)((DateTime.UtcNow.Ticks + 1) % int.MaxValue);
            var instrumentoPrueba = new instrumento
            {
                codigo = codigo,
                nombre = "Bajo Electrico",
                idCategoria = categoriaPrueba.id,
                funcional = true,
                ocupado = false,
                eliminado = false,
                estuche = false
            };

            setupContext.instrumentos.Add(instrumentoPrueba);
            await setupContext.SaveChangesAsync();

            var idGenerado = instrumentoPrueba.codigo;

            // Act (Paso 2: Llamar al metodo GetById del repositorio)
            using var testContext = GetDatabaseContext();
            var repository = new InstrumentoRepository(testContext);
            var resultado = repository.Get(i => i.codigo == idGenerado);

            // Assert (Paso 3: Comparar datos)
            Assert.NotNull(resultado);
            Assert.Equal(idGenerado, resultado.codigo);
            Assert.Equal("Bajo Electrico", resultado.nombre);

            // Limpieza (castroso fue esto)
            var instrumentoLimpieza = await testContext.instrumentos
                .FirstOrDefaultAsync(i => i.codigo == idGenerado);

            if (instrumentoLimpieza is not null)
            {
                testContext.instrumentos.Remove(instrumentoLimpieza);
                await testContext.SaveChangesAsync();
            }

            var categoriaLimpieza = await testContext.categoria
                .FirstOrDefaultAsync(c => c.id == categoriaPrueba.id);

            if (categoriaLimpieza is not null)
            {
                testContext.categoria.Remove(categoriaLimpieza);
                await testContext.SaveChangesAsync();
            }
        }
    }
}
