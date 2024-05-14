using Financials.Services.RequestsResponses.Conta;
using Financials.Services.RequestsResponses.Conta.Validators;
using FluentValidation.TestHelper;

namespace Financials.Tests.Services.Conta.Validators
{
    [TestFixture]
    public class AtualizarContaRequestValidatorTests
    {
        private AtualizarContaRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new AtualizarContaRequestValidator();
        }

        [TestCase("")]
        [TestCase("a")]
        public void Should_have_error_when_Nome_is_Invalid(string nome)
        {
            var request = new AtualizarContaRequest()
            {
                Nome = nome,
                SaldoInicial  = 0,
                Id = Guid.Empty,
                Tipo = Core.Enums.TipoConta.Corrente
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Nome);
        }

        [Test]
        public void Should_have_error_when_Id_is_Invalid()
        {
            var request = new AtualizarContaRequest()
            {
                Nome = "nome",
                SaldoInicial = 0,
                Id = Guid.Empty,
                Tipo = Core.Enums.TipoConta.Corrente
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Id);
        }
    }
}
