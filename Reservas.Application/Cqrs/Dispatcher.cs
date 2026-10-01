using System.Collections.Concurrent;

namespace Reservas.Application.Cqrs;

public sealed class Dispatcher(IServiceProvider servicios) : IDispatcher
{
    private static readonly ConcurrentDictionary<(Type, Type), object> InvocadoresDeComando = new();
    private static readonly ConcurrentDictionary<(Type, Type), object> InvocadoresDeQuery = new();

    public Task<TResultado> EnviarAsync<TResultado>(ICommand<TResultado> comando, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(comando);

        var invocador = (InvocadorDeComando<TResultado>)InvocadoresDeComando.GetOrAdd(
            (comando.GetType(), typeof(TResultado)),
            clave => Activator.CreateInstance(
                typeof(InvocadorDeComando<,>).MakeGenericType(clave.Item1, clave.Item2))!);

        return invocador.InvocarAsync(comando, servicios, ct);
    }

    public Task<TResultado> ConsultarAsync<TResultado>(IQuery<TResultado> query, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        var invocador = (InvocadorDeQuery<TResultado>)InvocadoresDeQuery.GetOrAdd(
            (query.GetType(), typeof(TResultado)),
            clave => Activator.CreateInstance(
                typeof(InvocadorDeQuery<,>).MakeGenericType(clave.Item1, clave.Item2))!);

        return invocador.InvocarAsync(query, servicios, ct);
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

    private static InvalidOperationException SinHandler(Type mensaje, string clase) =>
        new($"No hay handler registrado para la {clase} '{mensaje.Name}'. " +
            "Revisa que exista una clase que la atienda y que se haya llamado a AddApplication() en el host.");
}
