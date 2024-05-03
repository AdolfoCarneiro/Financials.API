using Financials.Core.Entity;
using Financials.Core.VO;
using Financials.Infrastructure.Configuraton;
using Financials.Services.Features.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework.Legacy;

namespace Financials.Tests.Services.Account
{
    [TestFixture]
    [Category("UnitTests")]
    public class LoginTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private Mock<IHttpContextAccessor> _contextAccessorMock;
        private Mock<IUserClaimsPrincipalFactory<ApplicationUser>> _userClaimsPrincipalFactoryMock;
        private Mock<IValidator<LoginRequest>> _validatorMock;
        private Mock<GerarTokens> _gerarTokensMock;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _contextAccessorMock = new Mock<IHttpContextAccessor>();
            _contextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();

            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object,
                _contextAccessorMock.Object,
                _userClaimsPrincipalFactoryMock.Object,
                null, null, null);

            _validatorMock = new Mock<IValidator<LoginRequest>>();

            var jwtConfig = new JWTConfiguration
            {
                SecretKey = "YourVerySecretKey",
                AccessTokenExpirationMinutes = 60,
                RefreshTokenExpirationMinutes = 120,
                Issuer = "YourIssuer",
                Audience = "YourAudience"
            };
            var jwtConfigMock = new Mock<IOptions<JWTConfiguration>>();
            jwtConfigMock.Setup(j => j.Value).Returns(jwtConfig);

            _gerarTokensMock = new Mock<GerarTokens>(_userManagerMock.Object, jwtConfigMock.Object); // Certifique-se de que os argumentos aqui estão corretos
        }

        [Test]
        public async Task Run_Should_Return_Errors_If_Request_Is_Invalid()
        {
            var request = new LoginRequest { Usuario = "test@example.com", Senha = "password" };
            var validationErrors = new List<ValidationFailure> { new ValidationFailure("Usuario", "Erro de validação") };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<LoginRequest>(), default))
                          .ReturnsAsync(new ValidationResult(validationErrors));

            var login = new Login(_userManagerMock.Object, _signInManagerMock.Object, _validatorMock.Object, _gerarTokensMock.Object);

            var result = await login.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error, Is.Not.Null);
                Assert.That(result.Error.CustomMessage, Is.EqualTo("Erro de validação"));
            });
        }

        [Test]
        public async Task Run_Should_Return_Error_If_User_Not_Found()
        {
            var request = new LoginRequest { Usuario = "unknown@example.com", Senha = "password" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(request.Usuario))
                            .ReturnsAsync((ApplicationUser)null);

            var login = new Login(_userManagerMock.Object, _signInManagerMock.Object, _validatorMock.Object, _gerarTokensMock.Object);

            var result = await login.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error, Is.Not.Null);
                Assert.That(result.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
            });
        }

        [Test]
        public async Task Run_Should_Return_Error_If_Login_Fails()
        {
            var user = new ApplicationUser { Email = "test@example.com", UserName = "test@example.com" };
            var request = new LoginRequest { Usuario = user.Email, Senha = "password" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(request.Usuario)).ReturnsAsync(user);
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, request.Senha, false, false))
                              .ReturnsAsync(SignInResult.Failed);

            var login = new Login(_userManagerMock.Object, _signInManagerMock.Object, _validatorMock.Object, _gerarTokensMock.Object);

            var result = await login.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error, Is.Not.Null);
                Assert.That(result.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
            });
        }

        [Test]
        public async Task Run_Should_Return_Token_If_Login_Successful()
        {
            var user = new ApplicationUser { Email = "test@example.com", UserName = "test@example.com" };
            var tokenVO = new TokenVO { AccessToken = "access_token", RefreshToken = "refresh_token", Expiration = DateTime.UtcNow.AddHours(1) };
            var request = new LoginRequest { Usuario = user.Email, Senha = "password" };
            _userManagerMock.Setup(um => um.FindByEmailAsync(request.Usuario)).ReturnsAsync(user);
            _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, request.Senha, false, false))
                              .ReturnsAsync(SignInResult.Success);
            _gerarTokensMock.Setup(gt => gt.Run(user)).ReturnsAsync(tokenVO);
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            var login = new Login(_userManagerMock.Object, _signInManagerMock.Object, _validatorMock.Object, _gerarTokensMock.Object);

            var result = await login.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.True);
                Assert.That(result.Data.Token, Is.EqualTo(tokenVO));
            });
        }

    }
}
