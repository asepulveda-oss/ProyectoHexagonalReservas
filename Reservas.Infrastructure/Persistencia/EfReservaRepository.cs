using Reservas.Domain;
using Microsoft.EntityFrameworkCore;


namespace Reservas.Infrastructure.Persistencia
{
    public sealed class EfReservaRepository(ReservasDbContext dbContext) : IReservaRepository
    {
        public async Task<IReadOnlyList<Reserva>> DeSalaAsync(Guid salaId, CancellationToken ct = default)
        {
            var reservasEntities = await dbContext.Reservas.
                Where(r => r.SalaId == salaId).ToListAsync();

            return reservasEntities.Select(r =>
              Reserva.Rehidratar(r.Id, r.SalaId, RangoHorario.Crear(r.Inicio, r.Fin))).ToList();
        }

        public async Task GuardarAsync(Reserva reserva, CancellationToken ct = default)
        {
            var reservaEntity = new ReservaEntity
            {
                Id = reserva.Id,
                SalaId = reserva.SalaId,
                Inicio = reserva.Rango.Inicio,
                Fin = reserva.Rango.Fin
            };

            dbContext.Reservas.Add(reservaEntity);

            await dbContext.SaveChangesAsync(ct);
        }
    }
}
