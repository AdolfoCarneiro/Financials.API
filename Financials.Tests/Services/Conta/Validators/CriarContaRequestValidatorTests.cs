using Financials.Services.RequestsResponses.Conta;
using Financials.Services.RequestsResponses.Conta.Validators;
using FluentValidation.TestHelper;

namespace Financials.Tests.Services.Conta.Validators
{
    [TestFixture]
    public class CriarContaRequestValidatorTests
    {
        private CriarContaRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CriarContaRequestValidator();
        }

        [TestCase("")]
        [TestCase("a")]
        public void Should_have_error_when_Nome_is_Invalid(string nome)
        {
            var request = new CriarContaRequest()
            {
                Nome = nome,
                SaldoInicial  = 0,
                Tipo = Core.Enums.TipoConta.Corrente
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Nome);
        }
    }
}
