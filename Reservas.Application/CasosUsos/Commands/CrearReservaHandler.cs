using Reservas.Domain;
using Reservas.Domain.Errores;

namespace Reservas.Application.CasosUsos.Commands
{
    public sealed class CrearReservaHandler(
        IReservaRepository reservaRepository) : Cqrs.ICommandHandler<CrearReserva, Guid>
    {
   

        public async Task<Guid> ManejarAsync(CrearReserva comando, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(comando);

            var rango = RangoHorario.Crear(comando.Inicio,comando.Fin);

            // vamos a buscar las sala de la reserva para validar 
            var reservas = await reservaRepository.DeSalaAsync(comando.SalaId,cancellationToken);

            var choque = reservas.FirstOrDefault(reserva => reserva.Rango.SeTraslapaCon(rango));

            if(choque is not null)
            {
                throw new SalaOcupadaException(comando.SalaId,rango,choque.Rango);
            }

            var reserva = Reserva.Crear(comando.SalaId, comando.Inicio, comando.Fin);

            // persistimos la reserva atraves del puerto sin saber cual es la fuente de datos
            await reservaRepository.GuardarAsync(reserva,cancellationToken);

            return reserva.Id;

        }
    }
}
