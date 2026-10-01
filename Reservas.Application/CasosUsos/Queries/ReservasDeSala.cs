namespace Reservas.Application.CasosUsos.Queries
{
    public sealed record ReservasDeSala(Guid SalaId) : Cqrs.IQuery<IReadOnlyList<ReservaVista>>;


    public sealed record ReservaVista(Guid Id, Guid SalaId, DateTime Inicio, DateTime Fin);
}
