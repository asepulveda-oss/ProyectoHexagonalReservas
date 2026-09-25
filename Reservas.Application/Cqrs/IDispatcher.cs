namespace Reservas.Application.Cqrs
{
    public interface IDispatcher
    {
        Task<TResultado> EnviarAsync<TResultado>(ICommand<TResultado> comando, CancellationToken ct = default);

        Task<TResultado> ConsultarAsync<TResultado>(IQuery<TResultado> query, CancellationToken ct = default);
    }
}
