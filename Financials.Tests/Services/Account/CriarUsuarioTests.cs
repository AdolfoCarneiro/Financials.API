using Financials.Core.Entity;
using Financials.Services.Features.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework.Legacy;

namespace Financials.Tests.Services.Account
{
    [TestFixture]
    public class CriarUsuarioTests
    {
        private Mock<UserManager<ApplicationUser>> userManagerMock;

        [SetUp]
        public void SetUp()
        {
            userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        }

        [Test]
        public async Task Should_return_Application_response()
        {
            var request = new UsuarioRequest();
            var criarUsuario = new CriarUsuario(userManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.That(result, Is.InstanceOf<ApplicationResponse<UsuarioResponse>>());
        }

    }
}
