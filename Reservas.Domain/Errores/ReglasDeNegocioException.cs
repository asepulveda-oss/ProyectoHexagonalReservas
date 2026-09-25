namespace Reservas.Domain.Errores
{
    public abstract class ReglasDeNegocioException : Exception
    {
        private static readonly IReadOnlyDictionary<string, object?> SinDetalles
            = new Dictionary<string, object?>();

        protected ReglasDeNegocioException(string codigo,string mensaje,
            IReadOnlyDictionary<string, object?> detalles = null): base(mensaje)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(codigo);

            Codigo = codigo;
            Detalles = detalles ?? SinDetalles;
                
        }

        public string Codigo { get; }
        public IReadOnlyDictionary<string, object?> Detalles { get; }
    }

    public abstract class ReservaInvalidaException: ReglasDeNegocioException
    {
        protected ReservaInvalidaException(
           string codigo,
           string mensaje,
           IReadOnlyDictionary<string, object?>? detalles = null)
           : base(codigo, mensaje, detalles)
        {
        }
    }

    public abstract class ConflictoDeEstadoException : ReglasDeNegocioException
    {
        protected ConflictoDeEstadoException(
            string codigo,
            string mensaje,
            IReadOnlyDictionary<string, object?>? detalles = null)
            : base(codigo, mensaje, detalles)
        {
        }
    }

}
