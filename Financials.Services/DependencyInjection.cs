using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Account.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Financials.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            return services;
        }
    }

}
