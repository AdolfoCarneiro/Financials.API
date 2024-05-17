using Financials.Services.RequestsResponses.Conta.Validators;
using Financials.Services.RequestsResponses.Conta;
using FluentValidation.TestHelper;

namespace Financials.Services.Tests.Services.Conta.Validators
{
    [TestFixture]
    [Category("UnitTests")]
    public class GetContaRequestValidatorTests
    {
        private GetContaRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new GetContaRequestValidator();
        }

        [Test]
        public void Should_have_error_when_Id_is_Invalid()
        {
            var request = new GetContaRequest()
            {
                ContaId = Guid.Empty,
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(conta => conta.ContaId);
        }
    }
}
