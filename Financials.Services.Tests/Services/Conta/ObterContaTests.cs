using Financials.Core.DTO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.Conta;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Conta
{
    [TestFixture]
    [Category("UnitTests")]
    public class ObterContaTests
    {
        private ObterConta _obterConta;
        private Mock<IContaRespositorio> _contaRepositorioMock;
        private Mock<IValidator<GetContaRequest>> _validatorMock;

        [SetUp]
        public void SetUp()
        {
            _contaRepositorioMock = new Mock<IContaRespositorio>();
            _validatorMock = new Mock<IValidator<GetContaRequest>>();
            _obterConta = new ObterConta(_contaRepositorioMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task Handle_ValidationFails_ReturnsErrorResponse()
        {
            var request = new GetContaRequest { ContaId = Guid.Empty };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new("ContaId", "ContaId inválido") }));

            var response = await _obterConta.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.ValidationError));
            });
        }

        [Test]
        public async Task Handle_AccountNotFound_ReturnsErrorResponse()
        {
            var request = new GetContaRequest { ContaId = Guid.NewGuid() };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync((Entity.Conta)null);

            var response = await _obterConta.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.NotFound));
            });
        }

        [Test]
        public async Task Handle_SuccessfulRetrieval_ReturnsSuccessResponseWithData()
        {
            var id = Guid.NewGuid();
            var contaMock = new Entity.Conta() { Id = id, Nome = "Nu", SaldoInicial = 0, Tipo = Core.Enums.TipoConta.Corrente };
            ContaDTO contaDtoMock =contaMock.ToMapper();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>())).ReturnsAsync(contaMock);

            var request = new GetContaRequest { ContaId = Guid.NewGuid() };
            var response = await _obterConta.Handle(request, CancellationToken.None);


            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                response.Data.Should().BeEquivalentTo(contaDtoMock);
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            var request = new GetContaRequest { ContaId = Guid.NewGuid() };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<GetContaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>())).ThrowsAsync(new Exception("Erro de teste"));

            var response = await _obterConta.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
            });
        }
    }
}

