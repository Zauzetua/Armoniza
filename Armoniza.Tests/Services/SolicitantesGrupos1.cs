using Armoniza.Domain.Entities;
using Armoniza.Infrastructure.Infrastructure.Data;
using Armoniza.Infrastructure.Repository;
using Armoniza.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Armoniza.Tests.Services
{
    public class SolicitantesGrupos1
    {
        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=Armoniza;Username=postgres;Password=1001")
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        private static string SufijoUnico() => Guid.NewGuid().ToString("N")[..8];

        [Fact]
        public async Task CP5_1_CrearGrupoYRegistrarSolicitanteConFk()
        {
            int grupoId = 0;
            int tipoUsuarioId = 0;
            int usuarioId = 0;

            try
            {
                using var context = GetDatabaseContext();

                var grupoRepository = new GrupoRepository(context);
                var usuarioRepository = new UsuarioRepository(context);
                var tipoUsuarioRepository = new TipoUsuarioRepository(context);
                var apartadosRepository = new ApartadosRepository(context);

                var tipoUsuarioService = new TipoUsuarioService(tipoUsuarioRepository, usuarioRepository);
                var usuarioService = new UsuarioService(usuarioRepository, tipoUsuarioService, apartadosRepository);
                var grupoService = new GrupoService(grupoRepository, usuarioRepository);


                var sufijo = SufijoUnico();
                var nuevoGrupo = new grupo { grupo1 = $"Grupo_Test_{sufijo}" };
                var addGrupoResult = grupoService.Add(nuevoGrupo);
                Assert.True(addGrupoResult.Success);
                grupoId = nuevoGrupo.id;

                var tipo = new tipoUsuario
                {
                    tipo = $"Tipo_Test_{sufijo}",
                    eliminado = false,
                    capacidadInstrumentos = 1
                };
                context.tipoUsuarios.Add(tipo);
                await context.SaveChangesAsync();
                tipoUsuarioId = tipo.id;

                var nuevoUsuario = new usuario
                {
                    nombreCompleto = "solicitante test",
                    correo = $"solicitante_{sufijo}@test.local",
                    idTipo = tipoUsuarioId,
                    idGrupo = grupoId,
                    eliminado = false
                };


                var addUsuarioResult = usuarioService.Add(nuevoUsuario);


                Assert.True(addUsuarioResult.Success);

                var registroEnDb = await context.usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.correo == nuevoUsuario.correo);
                Assert.NotNull(registroEnDb);
                Assert.Equal(grupoId, registroEnDb.idGrupo);
                usuarioId = registroEnDb.id;
            }
            finally
            {

                using var cleanupContext = GetDatabaseContext();
                if (usuarioId != 0)
                {
                    var usuarioDb = await cleanupContext.usuarios.FirstOrDefaultAsync(u => u.id == usuarioId);
                    if (usuarioDb is not null) cleanupContext.usuarios.Remove(usuarioDb);
                }
                if (grupoId != 0)
                {
                    var grupoDb = await cleanupContext.grupos.FirstOrDefaultAsync(g => g.id == grupoId);
                    if (grupoDb is not null) cleanupContext.grupos.Remove(grupoDb);
                }
                if (tipoUsuarioId != 0)
                {
                    var tipoDb = await cleanupContext.tipoUsuarios.FirstOrDefaultAsync(t => t.id == tipoUsuarioId);
                    if (tipoDb is not null) cleanupContext.tipoUsuarios.Remove(tipoDb);
                }

                await cleanupContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task CP5_2_TraerColeccionDeSolicitantesConInclude()
        {
            int grupoId = 0;
            int tipoUsuarioId = 0;
            List<int> usuarioIds = new();

            try
            {
                using var context = GetDatabaseContext();

                var grupoRepository = new GrupoRepository(context);
                var usuarioRepository = new UsuarioRepository(context);
                var tipoUsuarioRepository = new TipoUsuarioRepository(context);
                var apartadosRepository = new ApartadosRepository(context);

                var tipoUsuarioService = new TipoUsuarioService(tipoUsuarioRepository, usuarioRepository);
                var usuarioService = new UsuarioService(usuarioRepository, tipoUsuarioService, apartadosRepository);
                var grupoService = new GrupoService(grupoRepository, usuarioRepository);

                var sufijo = SufijoUnico();
                var nuevoGrupo = new grupo { grupo1 = $"Grupo_Test_{sufijo}" };
                var addGrupoResult = grupoService.Add(nuevoGrupo);
                Assert.True(addGrupoResult.Success);
                grupoId = nuevoGrupo.id;

                var tipo = new tipoUsuario
                {
                    tipo = $"Grupo_Test_{sufijo}",
                    eliminado = false,
                    capacidadInstrumentos = 1
                };
                context.tipoUsuarios.Add(tipo);
                await context.SaveChangesAsync();
                tipoUsuarioId = tipo.id;

                var usuario1 = new usuario
                {
                    nombreCompleto = "solicitante test 1",
                    correo = $"solicitante1_{sufijo}@test.local",
                    idTipo = tipoUsuarioId,
                    idGrupo = grupoId,
                    eliminado = false
                };
                var usuario2 = new usuario
                {
                    nombreCompleto = "solicitante test 2",
                    correo = $"solicitante2_{sufijo}@test.local",
                    idTipo = tipoUsuarioId,
                    idGrupo = grupoId,
                    eliminado = false
                };

                Assert.True(usuarioService.Add(usuario1).Success);
                Assert.True(usuarioService.Add(usuario2).Success);

                usuarioIds = await context.usuarios
                    .Where(u => u.idGrupo == grupoId)
                    .Select(u => u.id)
                    .ToListAsync();

                var grupoConUsuarios = grupoService.GetConUsuarios(g => g.id == grupoId);

                Assert.True(grupoConUsuarios.Success);
                Assert.NotNull(grupoConUsuarios.Data);
                Assert.NotNull(grupoConUsuarios.Data.usuario);
                Assert.Equal(2, grupoConUsuarios.Data.usuario.Count);
            }
            finally
            {

                using var cleanupContext = GetDatabaseContext();
                if (usuarioIds.Count > 0)
                {
                    var usuariosDb = await cleanupContext.usuarios.Where(u => usuarioIds.Contains(u.id)).ToListAsync();
                    cleanupContext.usuarios.RemoveRange(usuariosDb);
                }
                if (grupoId != 0)
                {
                    var grupoDb = await cleanupContext.grupos.FirstOrDefaultAsync(g => g.id == grupoId);
                    if (grupoDb is not null) cleanupContext.grupos.Remove(grupoDb);
                }
                if (tipoUsuarioId != 0)
                {
                    var tipoDb = await cleanupContext.tipoUsuarios.FirstOrDefaultAsync(t => t.id == tipoUsuarioId);
                    if (tipoDb is not null) cleanupContext.tipoUsuarios.Remove(tipoDb);
                }
                await cleanupContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task CP5_3_NoEliminarGrupoConSolicitantesAsignados()
        {
            int grupoId = 0;
            int tipoUsuarioId = 0;
            int usuarioId = 0;

            try
            {
                using var context = GetDatabaseContext();

                var grupoRepository = new GrupoRepository(context);
                var usuarioRepository = new UsuarioRepository(context);
                var tipoUsuarioRepository = new TipoUsuarioRepository(context);
                var apartadosRepository = new ApartadosRepository(context);

                var tipoUsuarioService = new TipoUsuarioService(tipoUsuarioRepository, usuarioRepository);
                var usuarioService = new UsuarioService(usuarioRepository, tipoUsuarioService, apartadosRepository);
                var grupoService = new GrupoService(grupoRepository, usuarioRepository);

                var sufijo = SufijoUnico();
                var nuevoGrupo = new grupo { grupo1 = $"Test_Delete_{sufijo}" };
                Assert.True(grupoService.Add(nuevoGrupo).Success);
                grupoId = nuevoGrupo.id;

                var tipo = new tipoUsuario
                {
                    tipo = $"Test_Delete_{sufijo}",
                    eliminado = false,
                    capacidadInstrumentos = 1
                };
                context.tipoUsuarios.Add(tipo);
                await context.SaveChangesAsync();
                tipoUsuarioId = tipo.id;

                var usuario = new usuario
                {
                    nombreCompleto = "solicitante test delete",
                    correo = $"solicitante_delete_{sufijo}@test.local",
                    idTipo = tipoUsuarioId,
                    idGrupo = grupoId,
                    eliminado = false
                };
                Assert.True(usuarioService.Add(usuario).Success);

                usuarioId = await context.usuarios
                    .Where(u => u.correo == usuario.correo)
                    .Select(u => u.id)
                    .FirstAsync();


                var deleteResult = await grupoService.Delete(grupoId);

                Assert.False(deleteResult.Success);
                Assert.Equal("No se puede eliminar el grupo porque tiene usuarios asignados", deleteResult.Message);
            }
            finally
            {

                using var cleanupContext = GetDatabaseContext();
                if (usuarioId != 0)
                {
                    var usuarioDb = await cleanupContext.usuarios.FirstOrDefaultAsync(u => u.id == usuarioId);
                    if (usuarioDb is not null) cleanupContext.usuarios.Remove(usuarioDb);
                }
                if (grupoId != 0)
                {
                    var grupoDb = await cleanupContext.grupos.FirstOrDefaultAsync(g => g.id == grupoId);
                    if (grupoDb is not null) cleanupContext.grupos.Remove(grupoDb);
                }
                if (tipoUsuarioId != 0)
                {
                    var tipoDb = await cleanupContext.tipoUsuarios.FirstOrDefaultAsync(t => t.id == tipoUsuarioId);
                    if (tipoDb is not null) cleanupContext.tipoUsuarios.Remove(tipoDb);
                }
                await cleanupContext.SaveChangesAsync();
            }
        }
    }
}
