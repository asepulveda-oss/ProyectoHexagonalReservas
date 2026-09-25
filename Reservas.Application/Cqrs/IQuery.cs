namespace Reservas.Application.Cqrs
{
    public interface IQuery<TResultado>;

    public interface IQueryHandler<in TQuery, TResultado>
        where TQuery : IQuery<TResultado>
    {
        Task<TResultado> 
            ManejarAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
