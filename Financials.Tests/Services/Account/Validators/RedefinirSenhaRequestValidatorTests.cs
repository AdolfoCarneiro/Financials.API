using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Account.Validators;
using FluentValidation.TestHelper;

namespace Financials.Tests.Services.Account.Validators
{
    [TestFixture]
    public class RedefinirSenhaRequestValidatorTests
    {
        private RedefinirSenhaRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RedefinirSenhaRequestValidator();
        }

        [Test]
        public void Should_have_error_when_UsuarioId_is_default()
        {
            var request = new RedefinirSenhaRequest { UsuarioId = Guid.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.UsuarioId).WithErrorMessage("O campo Id é obrigatório");
        }

        [Test]
        public void Should_have_error_when_NovaSenha_is_empty()
        {
            var request = new RedefinirSenhaRequest { NovaSenha = "" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.NovaSenha).WithErrorMessage("O campo Senha é obrigatório");
        }

        [Test]
        public void Should_have_error_when_NovaSenha_is_too_short()
        {
            var request = new RedefinirSenhaRequest { NovaSenha = "Abc1!" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.NovaSenha).WithErrorMessage("O campo Senha deve possuir pelo menos 8 caracteres");
        }

        [Test]
        public void Should_have_error_when_NovaSenha_is_too_long()
        {
            var request = new RedefinirSenhaRequest { NovaSenha = "Abc1!234567890123456789" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.NovaSenha).WithErrorMessage("O campo Senha deve possuir no máximo 16 caracteres.");
        }

        [Test]
        public void Should_have_error_when_NovaSenha_lacks_uppercase()
        {
            var request = new RedefinirSenhaRequest { NovaSenha = "abc1!2345" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.NovaSenha).WithErrorMessage("O campo Senha deve possuir pelo menos uma letra maiúscula.");
        }

        [Test]
        public void Should_have_error_when_NovaSenha_lacks_lowercase()
        {
            var request = new RedefinirSenhaRequest { NovaSenha = "ABC1!2345" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.NovaSenha).WithErrorMessage("O campo Senha deve possuir pelo menos uma letra minúscula.");
        }

        [Test]
        public void Should_have_error_when_NovaSenha_lacks_digit()
        {
            var request = new RedefinirSenhaRequest { NovaSenha = "ABCd!efgh" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.NovaSenha).WithErrorMessage("O campo Senha deve possuir pelo menos um número.");
        }

        [Test]
        public void Should_have_error_when_NovaSenha_lacks_special_character()
        {
            var request = new RedefinirSenhaRequest { NovaSenha = "ABCd12345" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.NovaSenha).WithErrorMessage("O campo Senha deve possuir pelo menos um caractere especial.");
        }

        [Test]
        public void Should_have_error_when_Token_is_empty()
        {
            var request = new RedefinirSenhaRequest { Token = "" };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Token).WithErrorMessage("O campo Token é obrigatório");
        }

    }
}
