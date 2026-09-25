using System.Collections.Concurrent;

namespace Reservas.Application.Cqrs;
    public sealed class Dispatcher(IServiceProvider services) : IDispatcher
    {
        private static readonly ConcurrentDictionary<(Type, Type), object> InvocadoresDeComando = new();
        private static readonly ConcurrentDictionary<(Type, Type), object> InvocadoresDeQuery = new();

        private static InvalidOperationException SinHandler(Type mensaje, string clase) =>
        new($"No hay handler registrado para la {clase} '{mensaje.Name}'. " +
            "Revisa que exista una clase que la atienda y que se haya llamado a AddApplication() en el host.");

    private abstract class InvocadorDeQuery<TResultado>
    {
        public abstract Task<TResultado> InvocarAsync(
            IQuery<TResultado> query, IServiceProvider servicios, CancellationToken ct);
    }

    private sealed class InvocadorDeQuery<TQuery, TResultado> : InvocadorDeQuery<TResultado>
      where TQuery : IQuery<TResultado>
    {
        public override Task<TResultado> InvocarAsync(
            IQuery<TResultado> query, IServiceProvider servicios, CancellationToken ct)
        {
            var handler = servicios.GetService(typeof(IQueryHandler<TQuery, TResultado>))
                              as IQueryHandler<TQuery, TResultado>
                          ?? throw SinHandler(typeof(TQuery), "query");

            return handler.ManejarAsync((TQuery)query, ct);
        }
    }

    private abstract class InvocadorDeComando<TResultado>
    {
        public abstract Task<TResultado> InvocarAsync(
            ICommand<TResultado> comando, IServiceProvider servicios, CancellationToken ct);
    }

    private sealed class InvocadorDeComando<TComando, TResultado> : InvocadorDeComando<TResultado>
        where TComando : ICommand<TResultado>
    {
        public override Task<TResultado> InvocarAsync(
            ICommand<TResultado> comando, IServiceProvider servicios, CancellationToken ct)
        {
            var handler = servicios.GetService(typeof(ICommandHandler<TComando, TResultado>))
                              as ICommandHandler<TComando, TResultado>
                          ?? throw SinHandler(typeof(TComando), "comando");

            return handler.ManejarAsync((TComando)comando, ct);
        }
    }


}

   
