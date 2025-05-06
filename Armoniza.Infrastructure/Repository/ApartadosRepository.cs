using Armoniza.Application.Common.Interfaces.Repositories;
using Armoniza.Application.Common.Models;
using Armoniza.Domain.Entities;
using Armoniza.Domain.Entities.Vistas;
using Armoniza.Infrastructure.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Armoniza.Infrastructure.Repository
{
    public class ApartadosRepository : Repository<apartado>, IApartadosRepository
    {
        private readonly ApplicationDbContext _context;
        public ApartadosRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task Delete(int id)
        {
            var apartado = _context.apartados.FirstOrDefault(x => x.id == id);
            if (apartado != null)
            {

                _context.SaveChanges();
            }
            return Task.CompletedTask;

        }

        public async Task<bool> LiberarApartadoAsync(int idApartado)
        {
            var param = new NpgsqlParameter("id_apartado", idApartado);

            var result = await _context
                .Set<LiberarApartadoResult>()
                .FromSqlRaw("SELECT liberar_apartado(@id_apartado)", param)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result?.liberar_apartado ?? false;
        }

        public async Task<int> CrearApartado(IEnumerable<int> idsInstrumentos, DateTime fechaRegreso, int idUsuario)
        {
            var detallesJson = JsonSerializer.Serialize(
                idsInstrumentos.Select(id => new { id })
            );

            var detallesParam = new NpgsqlParameter("detalles", detallesJson);
            var fechaParam = new NpgsqlParameter("fechaRegreso", fechaRegreso);
            var usuarioParam = new NpgsqlParameter("idUsuario", idUsuario);

            var result = await _context
                .Set<ApartadoCreadoResult>()
                .FromSqlRaw("SELECT crear_apartado(@detalles::jsonb, @fechaRegreso, @idUsuario)",
                            detallesParam, fechaParam, usuarioParam)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return result?.crear_apartado ?? 0;
        }

		public async Task<List<InstrumentoUsuario>> GetInstrumentosPorUsuario(int id)
		{
            var result = await _context.Set<InstrumentoUsuario>()
				.FromSqlRaw("SELECT * FROM obtener_instrumentos_por_usuario(@id)", new NpgsqlParameter("id", id))
				.AsNoTracking()
				.ToListAsync();

			if (result == null)
			{
				return new List<InstrumentoUsuario>();
			}
			return result;
		}


	}

}

