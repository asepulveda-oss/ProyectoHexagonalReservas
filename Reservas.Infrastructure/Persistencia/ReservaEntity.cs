namespace Reservas.Infrastructure.Persistencia
{
    public sealed class ReservaEntity
    {
        public Guid Id { get; set; }
        public Guid SalaId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
    }
}
