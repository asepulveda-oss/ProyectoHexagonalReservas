namespace Reservas.Domain
{
    public interface IReservaRepository
    {
        Task<IReadOnlyList<Reserva>> DeSalaAsync(Guid salaId, CancellationToken ct = default);
        Task GuardarAsync(Reserva reserva, CancellationToken ct = default);
    }
}
