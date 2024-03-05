using Financials.Infrastructure.Context;
using Financials.Infrastructure.Repositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financials.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<FinancialsDbContext>(options => options.UseSqlServer(defaultConnectionString));

            services.AddScoped<IFInancialsRepositorio, FinancialsRepositorio>();

            return services;
        }
    }
}
