using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.Fatura;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Fatura;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Fatura
{
    [TestFixture]
    [Category("UnitTests")]
    public class ObterFaturaTests
    {
        private ObterFatura _obterFatura;
        private Mock<IValidator<ObterFaturaRequest>> _validatorMock;
        private Mock<IDataFechamentoCartaoRepositorio> _dataFechamentoCartaoRepositorioMock;
        private Mock<ICartaoCreditoRepositorio> _cartaoCreditoRepositorioMock;
        private Mock<ITransacaoRepositorio> _transacaoRepositorioMock;

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator<ObterFaturaRequest>>();
            _dataFechamentoCartaoRepositorioMock = new Mock<IDataFechamentoCartaoRepositorio>();
            _cartaoCreditoRepositorioMock = new Mock<ICartaoCreditoRepositorio>();
            _transacaoRepositorioMock = new Mock<ITransacaoRepositorio>();
            _obterFatura = new ObterFatura(_validatorMock.Object, _dataFechamentoCartaoRepositorioMock.Object, _cartaoCreditoRepositorioMock.Object, _transacaoRepositorioMock.Object);
        }

        [Test]
        public async Task Handle_ValidationFails_ReturnsErrorResponse()
        {
            var request = new ObterFaturaRequest { CartaoId = Guid.Empty };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ObterFaturaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("CartaoId", "CartaoId é obrigatório") }));

            var response = await _obterFatura.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.ValidationError));
            });
        }

        [Test]
        public async Task Handle_CartaoNotFound_ReturnsErrorResponse()
        {
            var request = new ObterFaturaRequest
            {
                CartaoId = Guid.NewGuid(),
                DataReferencia = DateTime.UtcNow
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ObterFaturaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                                         .ReturnsAsync((Entity.CartaoCredito)null);

            var response = await _obterFatura.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.NotFound));
            });
        }

        [Test]
        public async Task Handle_SuccessfulFaturaRetrieval()
        {
            var cartaoMock = new Entity.CartaoCredito
            {
                Id = Guid.NewGuid(),
                Nome = "Cartão Teste",
                DataFechamento = new DateTime(2023, 10, 5),
                DataVencimento = new DateTime(2023, 10, 15),
                Limite = 5000
            };

            var transacoesMock = new List<Entity.Transacao>
            {
                new() { Valor = 100, Data = new DateTime(2023, 10, 6) },
                new() { Valor = 200, Data = new DateTime(2023, 10, 10) }
            };

            var request = new ObterFaturaRequest
            {
                CartaoId = cartaoMock.Id,
                DataReferencia = new DateTime(2023, 10, 10)
            };

            var alteracoesFechamentoMock = new List<Entity.DataFechamentoCartaoCredito>
            {
                new() {
                    CartaoCreditoId = cartaoMock.Id,
                    DataAlteracao = new DateTime(2023, 9, 1),
                    DataFechamentoAnterior = new DateTime(2023, 10, 10)
                }
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ObterFaturaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                                         .ReturnsAsync(cartaoMock);
            _dataFechamentoCartaoRepositorioMock.Setup(r => r.GetByExpression(It.IsAny<Expression<Func<Entity.DataFechamentoCartaoCredito, bool>>>()))
                                                .Returns(alteracoesFechamentoMock.AsQueryable());
            _transacaoRepositorioMock.Setup(r => r.GetByExpression(It.IsAny<Expression<Func<Entity.Transacao, bool>>>()))
                                     .Returns(transacoesMock.AsQueryable());

            var response = await _obterFatura.Handle(request, CancellationToken.None);

            var expectedFechamento = new DateTime(2023, 11, 10);
            var expectedVencimento = expectedFechamento.AddDays((cartaoMock.DataVencimento - cartaoMock.DataFechamento).Days);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                Assert.That(response.Data.Valor, Is.EqualTo(transacoesMock.Sum(t => t.Valor)));
                Assert.That(response.Data.Transacoes, Has.Count.EqualTo(transacoesMock.Count));
                Assert.That(response.Data.Fechamento, Is.EqualTo(expectedFechamento));
                Assert.That(response.Data.Vencimento, Is.EqualTo(expectedVencimento));
                Assert.That(response.Data.CartaoCredito.Id, Is.EqualTo(cartaoMock.Id));
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            var request = new ObterFaturaRequest
            {
                CartaoId = Guid.NewGuid(),
                DataReferencia = DateTime.UtcNow
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ObterFaturaRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                                         .ThrowsAsync(new Exception("Erro ao obter cartão"));

            var response = await _obterFatura.Handle(request, CancellationToken.None);

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
