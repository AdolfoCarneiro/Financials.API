using Azure.Core;
using Financials.Core.Entity;
using Financials.Infrastructure.Configuraton;
using Financials.Services.Features.Account;
using Financials.Services.RequestsResponses.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Financials.Tests.Services.Account
{
    [TestFixture]
    [Category("UnitTests")]
    public class GerarTokenTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<IOptions<JWTConfiguration>> _jwtConfigMock;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _jwtConfigMock = new Mock<IOptions<JWTConfiguration>>();
            _jwtConfigMock.Setup(c => c.Value).Returns(new JWTConfiguration
            {
                SecretKey = "afsdkjasjflxswafsdklk434orqiwup3457u-34oewir4irroqwiffv48mfs",
                AccessTokenExpirationMinutes = 60,
                RefreshTokenExpirationMinutes = 120,
                Issuer = "ExampleIssuer",
                Audience = "ExampleAudience"
            });
        }

        [Test]
        public async Task Handle_Should_Include_Required_Claims_In_Tokens()
        {
            var request = new GenerateTokenRequest() { User = new ApplicationUser { Id = "user-id" } };
            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string> { "Admin" });

            var service = new GerarTokens(_userManagerMock.Object, _jwtConfigMock.Object);
            var result = await service.Handle(request);

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.ReadJwtToken(result.AccessToken);
            var refreshToken = handler.ReadJwtToken(result.RefreshToken);

            Assert.Multiple(() =>
            {
                Assert.That(accessToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti && c.Value == request.User.Id), Is.Not.Null);
                Assert.That(accessToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role && c.Value == "Admin"), Is.Not.Null);
                Assert.That(refreshToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti && c.Value == request.User.Id), Is.Not.Null);
            });
        }

        [Test]
        public async Task Handle_Should_Sign_Tokens_With_Correct_Key_And_Algorithm()
        {
            var request = new GenerateTokenRequest() { User = new ApplicationUser { Id = "user-id" } };
            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());

            var service = new GerarTokens(_userManagerMock.Object, _jwtConfigMock.Object);
            var result = await service.Handle(request);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(result.AccessToken);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigMock.Object.Value.SecretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _jwtConfigMock.Object.Value.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtConfigMock.Object.Value.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = handler.ValidateToken(result.AccessToken, validationParameters, out var validatedToken);
            Assert.That(validatedToken, Is.Not.Null);
        }

        [Test]
        public async Task Handle_Should_Handle_Exceptions_When_Getting_Roles_Fails()
        {
            var request = new GenerateTokenRequest() { User = new ApplicationUser { Id = "user-id" } };
            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
                             .ThrowsAsync(new Exception("Failed to fetch roles"));

            var service = new GerarTokens(_userManagerMock.Object, _jwtConfigMock.Object);

            Assert.ThrowsAsync<Exception>(async () => await service.Handle(request));
        }

        [Test]
        public async Task Handle_Should_Set_Correct_Expiration_Times()
        {
            var request = new GenerateTokenRequest() { User = new ApplicationUser { Id = "user-id" } };
            _userManagerMock.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());

            var service = new GerarTokens(_userManagerMock.Object, _jwtConfigMock.Object);

            var result = await service.Handle(request);
            var expectedAccessTokenExpiration = DateTime.UtcNow.AddMinutes(60);
            var expectedRefreshTokenExpiration = DateTime.UtcNow.AddMinutes(120);

            Assert.That(result.Expiration, Is.EqualTo(expectedAccessTokenExpiration).Within(TimeSpan.FromSeconds(5)));
        }
    }
}
