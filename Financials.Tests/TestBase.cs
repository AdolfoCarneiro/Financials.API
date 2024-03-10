using Financials.Core.Entity;
using Financials.Infrastructure;
using Financials.Infrastructure.Context;
using Financials.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Financials.Tests
{
    public class TestBase
    {
        private IServiceScope? _scope;
        protected IConfiguration? _configuration;
        protected ServiceProvider? _serviceProvider;
        protected ServiceCollection? _serviceCollection;

        [OneTimeSetUp]
        public void BaseSetup()
        {
            _serviceCollection = new ServiceCollection();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json") // Provide the path to your configuration file
                .Build();

            _serviceCollection.AddSingleton<IConfiguration>(provider => _configuration);

            _serviceCollection.AddInfrastructure(_configuration);
            _serviceCollection.AddValidators();
            _serviceCollection.AddServices();

            _serviceCollection.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<FinancialsDbContext>()
                .AddDefaultTokenProviders();

            // Fazer o build do provedor de serviços
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            // Criar um escopo de serviço para resolver as dependências durante o teste
            _scope = _serviceProvider.CreateScope();
        }
    }
}
