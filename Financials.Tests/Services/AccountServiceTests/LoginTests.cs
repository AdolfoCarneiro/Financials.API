using Financials.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Financials.Tests.Services.AccountServiceTests
{
    [TestFixture]
    public class LoginTests : TestBase
    {
        IAccountService _accountService;
        [SetUp]
        public void Setup()
        {
            _accountService = _serviceProvider.GetRequiredService<IAccountService>();
        }

        [Test]
        public async Task Autenticar_WithValidCredentials_ShouldReturnSuccess()
        {

        }
    }
}
