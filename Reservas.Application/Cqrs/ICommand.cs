namespace Reservas.Application.Cqrs
{
    public interface ICommand<TResultado>;

    public interface ICommandHandler<in TComando, TResultado>
        where TComando : ICommand<TResultado>
    {
        Task<TResultado> 
            ManejarAsync(TComando comando, CancellationToken cancellationToken = default);
    }
}
