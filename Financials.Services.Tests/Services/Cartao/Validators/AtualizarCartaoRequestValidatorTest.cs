using Financials.Services.RequestsResponses.Cartao;
using Financials.Services.RequestsResponses.Cartao.Validators;
using FluentValidation.Results;

namespace Financials.Services.Tests.Validators
{
    [TestFixture]
    [Category("UnitTests")]
    public class AtualizarCartaoRequestValidatorTests
    {
        private AtualizarCartaoRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new AtualizarCartaoRequestValidator();
        }

        [Test]
        public void Validate_IdIsEmpty_ReturnsValidationError()
        {
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.Empty,
                Nome = "Cartão Teste",
                DataFechamento = DateTime.UtcNow
            };

            var result = _validator.Validate(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Has.Exactly(1).Matches<ValidationFailure>(f => f.PropertyName == "Id" && f.ErrorMessage == "Id inválido"));
            });
        }

        [Test]
        public void Validate_NomeIsEmpty_ReturnsValidationError()
        {
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.NewGuid(),
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
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.NewGuid(),
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
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.NewGuid(),
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
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Cartão Válido",
                DataFechamento = DateTime.UtcNow
            };

            var result = _validator.Validate(request);

            Assert.That(result.IsValid, Is.True);
        }
    }
}
