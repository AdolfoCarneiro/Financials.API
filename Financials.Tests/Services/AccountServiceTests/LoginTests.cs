using Moq;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using Financials.Core.Entity;
using Financials.Infrastructure.Repositorio;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.Services;
using Financials.Infrastructure.Configuraton;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NUnit.Framework.Legacy;

namespace Financials.Tests.Services.AccountServiceTests
{
    [TestFixture]
    public class LoginTests
    {
        private Mock<UserManager<ApplicationUser>> userManagerMock;
        private Mock<RoleManager<IdentityRole>> roleManagerMock;
        private Mock<IFInancialsRepositorio> repositorioMock;
        private Mock<SignInManager<ApplicationUser>> signInManagerMock;
        private Mock<IValidator<LoginRequest>> loginValidatorMock;
        private Mock<IValidator<UsuarioRequest>> usuarioValidatorMock;
        private IOptions<JWTConfiguration> jwtConfiguration;

        [SetUp]
        public void SetUp()
        {
            userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            repositorioMock = new Mock<IFInancialsRepositorio>();
            signInManagerMock = new Mock<SignInManager<ApplicationUser>>(userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), null, null, null, null);
            loginValidatorMock = new Mock<IValidator<LoginRequest>>();
            usuarioValidatorMock = new Mock<IValidator<UsuarioRequest>>();
            jwtConfiguration = new OptionsWrapper<JWTConfiguration>(new JWTConfiguration());
        }

        [Test]
        public async Task Autenticar_WithValidCredentials_ShouldReturnSuccess()
        {
            loginValidatorMock.Setup(v => v.ValidateAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var service = new AccountService(
                userManagerMock.Object,
                roleManagerMock.Object,
                repositorioMock.Object,
                signInManagerMock.Object,
                loginValidatorMock.Object,
                usuarioValidatorMock.Object,
                jwtConfiguration);

            var result = await service.Autenticar(new LoginRequest
            {
                Senha = "Pass123$",
                Usuario = "adolfo@email.com"
            });

            ClassicAssert.IsNotNull(result);
        }
    }

}
