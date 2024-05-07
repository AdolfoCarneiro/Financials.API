using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Conta;
using Financials.Services.RequestsResponses.Conta.Validators;
using FluentValidation.TestHelper;

namespace Financials.Tests.Services.Account.Validators
{
    [TestFixture]
    public class ContaRequestValidatorTests
    {
        private ContaRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new ContaRequestValidator();
        }

        [TestCase("")]
        [TestCase("a")]
        public void Should_have_error_when_Nome_is_Invalid(string nome)
        {
            var request = new ContaRequest()
            {
                Nome = nome,
                Id = Guid.NewGuid(),
                SaldoInicial  = 0,
                Tipo = Core.Enums.TipoConta.Corrente
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Nome);
        }
    }
}
