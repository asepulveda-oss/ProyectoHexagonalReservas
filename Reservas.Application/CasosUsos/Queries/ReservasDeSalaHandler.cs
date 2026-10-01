using Reservas.Domain;

namespace Reservas.Application.CasosUsos.Queries
{
    public sealed class ReservasDeSalaHandler(
        IReservaRepository reservaRepository) : Cqrs.IQueryHandler<ReservasDeSala,IReadOnlyList<ReservaVista>>
    {
        public async Task<IReadOnlyList<ReservaVista>> ManejarAsync(ReservasDeSala query, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(query);

            var reservas = await reservaRepository.DeSalaAsync(query.SalaId, ct);

            return reservas.Select(r => new ReservaVista(r.Id,r.SalaId, r.Rango.Inicio
                ,r.Rango.Fin)).ToList();
        }
    }
}
