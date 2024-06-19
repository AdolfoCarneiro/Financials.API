using Financials.Services.RequestsResponses.Cartao;
using Financials.Services.RequestsResponses.Cartao.Validators;
using FluentValidation.Results;

namespace Financials.Services.Tests.Services.Cartao.Validators
{
    [TestFixture]
    [Category("UnitTests")]
    public class ResgistrarCartaoRequestValidatorTests
    {
        private RegistrarCartaoRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new RegistrarCartaoRequestValidator();
        }

        [Test]
        public void Validate_NomeIsEmpty_ReturnsValidationError()
        {
            var request = new RegistrarCartaoRequest
            {
                Nome = string.Empty,
                DataFechamento = DateTime.UtcNow
            };

            var result = _validator.Validate(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Has.Exactly(1).Matches<ValidationFailure>(f => f.PropertyName == "Nome" && f.ErrorMessage == "Nome é obrigatório"));
            });
        }

        [Test]
        public void Validate_NomeHasLessThan2Characters_ReturnsValidationError()
        {
            var request = new RegistrarCartaoRequest
            {
                Nome = "A",
                DataFechamento = DateTime.UtcNow
            };

            var result = _validator.Validate(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Has.Exactly(1).Matches<ValidationFailure>(f => f.PropertyName == "Nome" && f.ErrorMessage == "Nome precisa de pelo menos 2 carateres"));
            });
        }

        [Test]
        public void Validate_DataFechamentoIsEmpty_ReturnsValidationError()
        {
            var request = new RegistrarCartaoRequest
            {
                Nome = "Cartão Válido",
                DataFechamento = default(DateTime) // Default será 01/01/0001 00:00:00
            };

            var result = _validator.Validate(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Has.Exactly(1).Matches<ValidationFailure>(f => f.PropertyName == "DataFechamento" && f.ErrorMessage == "Data de fechamento é obrigatória"));
            });
        }

        [Test]
        public void Validate_ValidRequest_PassesValidation()
        {
            var request = new RegistrarCartaoRequest
            {
                Nome = "Cartão Válido",
                DataFechamento = DateTime.UtcNow
            };

            var result = _validator.Validate(request);

            Assert.That(result.IsValid, Is.True);
        }
    }
}
