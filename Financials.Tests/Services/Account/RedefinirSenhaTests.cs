using Financials.Core.Entity;
using Financials.Services.Features.Account;
using Financials.Services.RequestsResponses.Account;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Financials.Tests.Services.Account
{
    [TestFixture]
    [Category("UnitTests")]
    public class RedefinirSenhaTests
    {

        private Mock<IValidator<RedefinirSenhaRequest>> _validatorMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;

        [SetUp]
        public void Setup()
        {
            _validatorMock = new Mock<IValidator<RedefinirSenhaRequest>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        }

        [Test]
        public async Task Handle_Should_Return_Errors_If_Validation_Fails()
        {
            var request = new RedefinirSenhaRequest();
            var validationFailures = new List<ValidationFailure>
            {
                new("Token", "Token é inválido")
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            var redefinirSenha = new RedefinirSenha(_validatorMock.Object, _userManagerMock.Object);

            var result = await redefinirSenha.Handle(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error, Is.Not.Null);
                Assert.That(result.Error.CustomMessage, Is.EqualTo("Token é inválido"));
            });

        }

        [Test]
        public async Task Handle_Should_Return_Error_If_User_Not_Found()
        {
            var request = new RedefinirSenhaRequest { UsuarioId = Guid.NewGuid(), Token = "some-token", NovaSenha = "newPassword123" };
            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                          .ReturnsAsync(new ValidationResult());

            _userManagerMock.Setup(um => um.FindByIdAsync(request.UsuarioId.ToString()))
                            .ReturnsAsync((ApplicationUser)null);

            var redefinirSenha = new RedefinirSenha(_validatorMock.Object, _userManagerMock.Object);

            var result = await redefinirSenha.Handle(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Is.EqualTo("Usuario não encontrado"));
            });
        }

        [Test]
        public async Task Handle_Should_Return_Error_If_Password_Reset_Fails()
        {
            var user = new ApplicationUser { Id = "user-id" };
            var request = new RedefinirSenhaRequest { UsuarioId = Guid.NewGuid(), Token = "valid-token", NovaSenha = "newPassword123" };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                          .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.FindByIdAsync(request.UsuarioId.ToString()))
                            .ReturnsAsync(user);

            _userManagerMock.Setup(um => um.ResetPasswordAsync(user, request.Token, request.NovaSenha))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Falha ao redefinir senha" }));

            var redefinirSenha = new RedefinirSenha(_validatorMock.Object, _userManagerMock.Object);

            var result = await redefinirSenha.Handle(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Is.EqualTo("Erro ao redefinir senha do usuário"));
            });
        }


    }
}
