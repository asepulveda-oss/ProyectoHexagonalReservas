namespace Reservas.Domain.Errores
{
    public sealed class SalaOcupadaException : ConflictoDeEstadoException
    {
        public SalaOcupadaException(Guid salaId, RangoHorario rangoPedido, RangoHorario rangoOcupado)
       : base("sala.ocupada",
              $"La sala {salaId} ya tiene una reserva de {rangoOcupado} que se cruza " +
              $"con el tramo pedido ({rangoPedido}).",
              new Dictionary<string, object?>
              {
                  ["salaId"] = salaId,
                  ["inicioPedido"] = rangoPedido.Inicio,
                  ["finPedido"] = rangoPedido.Fin,
                  // Devolver el tramo que estorba, y no solo decir "está
                  // ocupada", es lo que permite que un frontend muestre
                  // "ocupada de 9 a 11, prueba después de las 11" en vez de un
                  // error genérico. Sale gratis: el caso de uso ya tenía el dato
                  // en la mano cuando detectó el choque.
                  ["inicioOcupado"] = rangoOcupado.Inicio,
                  ["finOcupado"] = rangoOcupado.Fin,
              })
        {
            SalaId = salaId;
            RangoPedido = rangoPedido;
            RangoOcupado = rangoOcupado;
        }

        /// <summary>Sala en conflicto.</summary>
        public Guid SalaId { get; }

        /// <summary>El tramo que se quiso reservar.</summary>
        public RangoHorario RangoPedido { get; }

        /// <summary>El tramo ya reservado que lo impide.</summary>
        public RangoHorario RangoOcupado { get; }
    }
}
