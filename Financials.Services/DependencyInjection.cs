using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Financials.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var allTypes = assembly.GetTypes()
            .Where(p => p.Namespace != null && p.Namespace.Contains("Financials.Services.Features") && p.IsClass && !p.IsAbstract);

            foreach (var type in allTypes)
            {
                services.AddScoped(type);
            }

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var validatorType = typeof(IValidator<>);
            var validatorTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType))
                .ToList();

            foreach (var type in validatorTypes)
            {
                var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                }
            }

            return services;
        }
    }

}
