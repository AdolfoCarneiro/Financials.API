using Financials.Services.RequestsResponses.Account;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Financials.Services
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Registra todos os Services por convenção (I{Name}Service -> {Name}Service)
            var serviceTypes = assembly.GetTypes()
                .Where(t => t.IsClass
                    && !t.IsAbstract
                    && t.Namespace != null
                    && t.Namespace.StartsWith("Financials.Services.Services.Implementations"))
                .ToList();

            foreach (var implementationType in serviceTypes)
            {
                var interfaceType = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(LoginRequest).Assembly);
            return services;
        }
    }
}
