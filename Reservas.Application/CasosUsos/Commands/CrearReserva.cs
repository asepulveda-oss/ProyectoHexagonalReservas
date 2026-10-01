namespace Reservas.Application.CasosUsos.Commands
{
    public sealed record CrearReserva(
        Guid SalaId,
        DateTime Inicio,
        DateTime Fin) : Cqrs.ICommand<Guid>;
}
