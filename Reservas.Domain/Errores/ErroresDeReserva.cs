namespace Reservas.Domain.Errores
{
    public sealed class RangoInvertidoException : ReservaInvalidaException
    {
        public RangoInvertidoException(DateTime inicio, DateTime fin)
        : base("reserva.rango_invertido",
               $"El fin ({fin:yyyy-MM-dd HH:mm} UTC) debe ser posterior al inicio " +
               $"({inicio:yyyy-MM-dd HH:mm} UTC).",
               new Dictionary<string, object?>
               {
                   ["inicio"] = inicio,
                   ["fin"] = fin,
               })
            {
                Inicio = inicio;
                Fin = fin;
            }

        public DateTime Inicio { get; }

        public DateTime Fin { get; }
    }

    public sealed class DuracionFueraDeRangoException : ReservaInvalidaException
    {
        public DuracionFueraDeRangoException(TimeSpan duracion, TimeSpan minima, TimeSpan maxima)
            : base("reserva.duracion_fuera_de_rango",
                   $"La reserva dura {duracion.TotalMinutes:0} minutos y debe durar entre " +
                   $"{minima.TotalMinutes:0} minutos y {maxima.TotalHours:0.#} horas.",
                   new Dictionary<string, object?>
                   {
                       ["duracionMinutos"] = duracion.TotalMinutes,
                       ["minimoMinutos"] = minima.TotalMinutes,
                       ["maximoMinutos"] = maxima.TotalMinutes,
                   })
        {
            Duracion = duracion;
            Minima = minima;
            Maxima = maxima;
        }

        public TimeSpan Duracion { get; }

        public TimeSpan Minima { get; }

        public TimeSpan Maxima { get; }

        /// <summary>
        /// ¿Se pasó por corta o por larga?
        ///
        /// Es la clase de pregunta que un cliente necesita responder para escribir un
        /// mensaje útil, y que sería imposible sin los datos tipados.
        /// </summary>
        public bool EsDemasiadoCorta => Duracion < Minima;
    }

    public sealed class ReservaEnElPasadoException : ReservaInvalidaException
    {
        public ReservaEnElPasadoException(DateTime inicio, DateTime ahora)
            : base("reserva.en_el_pasado",
                   $"La reserva empieza {inicio:yyyy-MM-dd HH:mm} UTC, que ya pasó " +
                   $"(ahora son las {ahora:yyyy-MM-dd HH:mm} UTC).",
                   new Dictionary<string, object?>
                   {
                       ["inicio"] = inicio,
                       ["ahora"] = ahora,
                   })
        {
            Inicio = inicio;
            Ahora = ahora;
        }

        public DateTime Inicio { get; }

        public DateTime Ahora { get; }
    }

    public sealed class SalaRequeridaException : ReservaInvalidaException
    {
        public SalaRequeridaException()
            : base("reserva.sala_requerida", "La reserva debe indicar una sala.")
        {
        }
    }
}




