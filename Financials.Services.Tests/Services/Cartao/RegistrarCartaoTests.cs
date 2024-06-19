using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.CartaoCredito;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Cartao;
using FluentValidation.Results;
using Moq;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Cartao
{
    [TestFixture]
    [Category("UnitTests")]
    public class RegistrarCartaoTests
    {
        private RegistrarCartao _registrarCartao;
        private Mock<IValidator<RegistrarCartaoRequest>> _validatorMock;
        private Mock<ICartaoCreditoRepositorio> _cartaoCreditoRepositorioMock;

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator<RegistrarCartaoRequest>>();
            _cartaoCreditoRepositorioMock = new Mock<ICartaoCreditoRepositorio>();
            _registrarCartao = new RegistrarCartao(_validatorMock.Object, _cartaoCreditoRepositorioMock.Object);
        }

        [Test]
        public async Task Handle_ValidationFails_ReturnsErrorResponse()
        {
            var request = new RegistrarCartaoRequest
            {
                Nome = "",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegistrarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nome", "Nome é obrigatório") }));

            var response = await _registrarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.ValidationError));
            });
        }

        [Test]
        public async Task Handle_CartaoRegisteredSuccessfully_ReturnsSuccessResponse()
        {
            var request = new RegistrarCartaoRequest
            {
                Nome = "Cartão Teste",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };

            var cartaoMock = new Entity.CartaoCredito
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                DataFechamento = request.DataFechamento,
                DataVencimento = request.DataVencimento,
                Limite = request.Limite
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegistrarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.CartaoCredito>()))
                                         .ReturnsAsync(cartaoMock);

            var response = await _registrarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                Assert.That(response.Data.Id, Is.EqualTo(cartaoMock.Id));
                Assert.That(response.Data.Nome, Is.EqualTo(cartaoMock.Nome));
                Assert.That(response.Data.DataFechamento, Is.EqualTo(cartaoMock.DataFechamento));
                Assert.That(response.Data.DataVencimento, Is.EqualTo(cartaoMock.DataVencimento));
                Assert.That(response.Data.Limite, Is.EqualTo(cartaoMock.Limite));
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            var request = new RegistrarCartaoRequest
            {
                Nome = "Cartão Teste",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegistrarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.CartaoCredito>()))
                                         .ThrowsAsync(new Exception("Erro ao inserir cartão"));

            var response = await _registrarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
            });
        }
    }
}
