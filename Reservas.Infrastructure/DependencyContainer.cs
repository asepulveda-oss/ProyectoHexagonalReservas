using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservas.Domain;
using Reservas.Infrastructure.Persistencia;

namespace Reservas.Infrastructure
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {

            var cadenaConexion = configuration.GetConnectionString("Reservas")
                ?? "Data Source=reservas.db";

            services.AddDbContext<ReservasDbContext>(options =>
                options.UseSqlite(cadenaConexion));

            services.AddScoped<IReservaRepository,EfReservaRepository>();

            return services;
        }
    }
}
