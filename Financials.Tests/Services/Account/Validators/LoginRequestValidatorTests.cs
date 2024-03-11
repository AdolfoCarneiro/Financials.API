using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Account.Validators;
using FluentValidation.TestHelper;

namespace Financials.Tests.Services.Account.Validators
{
    [TestFixture]
    public class LoginRequestValidatorTests
    {
        private LoginRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new LoginRequestValidator();
        }

        [TestCase("")]
        public void Should_have_error_when_Usuario_is_Invalid(string usuario)
        {
            var request = new LoginRequest()
            {
                Usuario = usuario
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Usuario);
        }

        [TestCase("")]
        public void Should_have_error_when_Senhad_is_Invalid(string senha)
        {
            var request = new LoginRequest()
            {
                Senha = senha
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Senha);
        }
    }
}
