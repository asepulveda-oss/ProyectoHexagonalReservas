namespace Reservas.API
{
    public sealed record CrearReservaRequest(Guid SalaId,
        DateTime Inicio,
        DateTime Fin);
}
