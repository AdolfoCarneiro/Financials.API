using Financials.Infrastructure.Configuraton;
using Financials.Infrastructure.Context;
using Financials.Infrastructure.HttpService;
using Financials.Infrastructure.Repositorio.Implementacoes;
using Financials.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Financials.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserContext, UserContext>();

            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FinancialsDbContext>((provider, options) =>
            {
                var userContext = provider.GetService<IUserContext>();
                options.UseSqlServer(defaultConnectionString);
                options.UseLazyLoadingProxies();
            });

            services.Configure<JWTConfiguration>(opt => configuration.GetSection("Jwt").Bind(opt));

            services.AddRepositories();

            return services;
        }

        public static IServiceCollection AddInitialSeedData(this IServiceCollection services)
        {
            services.AddScoped<ISeedInitialUserAndRoles, SeedInitialUserAndRoles>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var baseType = typeof(RepositorioGenerico<>);

            var repositoryTypes = assembly.GetTypes()
            .Where(t => t.Namespace != null && t.Namespace.StartsWith("Financials.Infrastructure.Repositorio")
                    && t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == baseType
            ).ToList();

            foreach (var type in repositoryTypes)
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == $"I{type.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }

            return services;
        }
    }
}
