using Financials.Services.Interfaces;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Account.Validators;
using Financials.Services.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace Financials.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<LoginRequest>,LoginRequestValidator>();
            return services;
        }
    }

}
