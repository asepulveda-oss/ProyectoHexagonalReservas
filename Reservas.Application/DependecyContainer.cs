using Microsoft.Extensions.DependencyInjection;
using Reservas.Application.Cqrs;

namespace Reservas.Application
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDispatcher,Dispatcher>();



            var interfacesDeHandler = new[] { typeof(ICommandHandler<,>), typeof(IQueryHandler<,>) };

            var registros =
                from tipo in typeof(DependecyContainer).Assembly.GetTypes()
                where tipo is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false }
                from contrato in tipo.GetInterfaces()
                where contrato.IsGenericType
                      && interfacesDeHandler.Contains(contrato.GetGenericTypeDefinition())
                select (contrato, tipo);

            foreach (var (contrato, tipo) in registros)
            {
                services.AddScoped(contrato, tipo);
            }


            return services;
        }
    }
}
