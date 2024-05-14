using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.Conta;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using FluentValidation.Results;
using Moq;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Conta
{
    [TestFixture]
    [Category("UnitTests")]
    public class AtualizarContaTests
    {
        private AtualizarConta _atualizarConta;
        private Mock<IContaRespositorio> _contaRepositorioMock;
        private Mock<IValidator<AtualizarContaRequest>> _validatorMock;

        [SetUp]
        public void SetUp()
        {
            _contaRepositorioMock = new Mock<IContaRespositorio>();
            _validatorMock = new Mock<IValidator<AtualizarContaRequest>>();
            _atualizarConta = new AtualizarConta(_contaRepositorioMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task Handle_ValidationFails_ReturnsErrorResponse()
        {
            var request = new AtualizarContaRequest() { Id = Guid.Empty, Nome = string.Empty };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("campo", "erro de validação") }));

            var response = await _atualizarConta.Handle(request);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.ValidationError));
            });

        }

        [Test]
        public async Task Handle_AccountNotFound_ReturnsErrorResponse()
        {
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Entity.Conta)null);

            var request = new AtualizarContaRequest();
            var response = await _atualizarConta.Handle(request);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.NotFound));
            });
        }

        [Test]
        public async Task Handle_SuccessfulUpdate_ReturnsSuccessResponseWithData()
        {
            var contaMock = new Entity.Conta {};
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(contaMock);
            _contaRepositorioMock.Setup(r => r.Update(It.IsAny<Entity.Conta>())).ReturnsAsync(contaMock);

            var request = new AtualizarContaRequest();
            var response = await _atualizarConta.Handle(request);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Data, Is.Not.Null);
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>())).ThrowsAsync(new Exception("Erro de teste"));

            var request = new AtualizarContaRequest();
            var response = await _atualizarConta.Handle(request);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
            });
        }

    }
}
