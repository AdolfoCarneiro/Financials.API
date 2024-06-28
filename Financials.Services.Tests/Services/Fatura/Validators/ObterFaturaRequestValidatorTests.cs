using Financials.Services.RequestsResponses.Fatura.Validators;
using Financials.Services.RequestsResponses.Fatura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace Financials.Services.Tests.Services.Fatura.Validators
{
    [TestFixture]
    [Category("UnitTests")]
    public class ObterFaturaRequestValidatorTests
    {
        private ObterFaturaRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ObterFaturaRequestValidator();
        }

        [Test]
        public void Validate_CartaoIdIsEmpty_ReturnsValidationError()
        {
            var request = new ObterFaturaRequest
            {
                CartaoId = Guid.Empty
            };

            var result = _validator.Validate(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Errors, Has.Exactly(1).Matches<ValidationFailure>(f => f.PropertyName == "CartaoId"));
            });
        }

        [Test]
        public void Validate_CartaoIdIsValid_PassesValidation()
        {
            var request = new ObterFaturaRequest
            {
                CartaoId = Guid.NewGuid()
            };

            var result = _validator.Validate(request);

            Assert.That(result.IsValid, Is.True);
        }
    }
}
