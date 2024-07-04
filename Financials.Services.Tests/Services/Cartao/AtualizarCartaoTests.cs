using Financials.Core.Entity;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.CartaoCredito;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Cartao;
using FluentValidation.Results;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Cartao
{
    [TestFixture]
    [Category("UnitTests")]
    public class AtualizarCartaoTests
    {
        private AtualizarCartao _atualizarCartao;
        private Mock<ICartaoCreditoRepositorio> _cartaoCreditoRepositorioMock;
        private Mock<IDataFechamentoCartaoRepositorio> _dataFechamentoCartaoRepositorioMock;
        private Mock<IValidator<AtualizarCartaoRequest>> _validatorMock;

        [SetUp]
        public void SetUp()
        {
            _cartaoCreditoRepositorioMock = new Mock<ICartaoCreditoRepositorio>();
            _dataFechamentoCartaoRepositorioMock = new Mock<IDataFechamentoCartaoRepositorio>();
            _validatorMock = new Mock<IValidator<AtualizarCartaoRequest>>();
            _atualizarCartao = new AtualizarCartao(_cartaoCreditoRepositorioMock.Object, _dataFechamentoCartaoRepositorioMock.Object, _validatorMock.Object);
        }

        [Test]
        public async Task Handle_ValidationFails_ReturnsErrorResponse()
        {
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.NewGuid(),
                Nome = "",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nome", "Nome é obrigatório") }));

            var response = await _atualizarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.ValidationError));
                _cartaoCreditoRepositorioMock.Verify(r => r.BeginTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.RollbackTransactionAsync(), Times.Never);
                _cartaoCreditoRepositorioMock.Verify(r => r.CommitTransactionAsync(), Times.Never);
            });
        }

        [Test]
        public async Task Handle_CartaoNotFound_ReturnsErrorResponse()
        {
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Cartão Teste",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                                         .ReturnsAsync((Entity.CartaoCredito)null);

            var response = await _atualizarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.NotFound));
                _cartaoCreditoRepositorioMock.Verify(r => r.BeginTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.RollbackTransactionAsync(), Times.Never);
                _cartaoCreditoRepositorioMock.Verify(r => r.CommitTransactionAsync(), Times.Never);
            });
        }

        [Test]
        public async Task Handle_SuccessfulUpdate_NoDateChange()
        {
            var cartaoMock = new Entity.CartaoCredito
            {
                Id = Guid.NewGuid(),
                Nome = "Cartão Teste",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };

            var request = new AtualizarCartaoRequest
            {
                Id = cartaoMock.Id,
                Nome = "Cartão Atualizado",
                DataFechamento = cartaoMock.DataFechamento,
                DataVencimento = cartaoMock.DataVencimento,
                Limite = 10000
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                                         .ReturnsAsync(cartaoMock);
            _cartaoCreditoRepositorioMock.Setup(r => r.Update(It.IsAny<Entity.CartaoCredito>()))
                                         .ReturnsAsync(cartaoMock);

            var response = await _atualizarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                Assert.That(response.Data.Id, Is.EqualTo(cartaoMock.Id));
                Assert.That(response.Data.Nome, Is.EqualTo(request.Nome));
                Assert.That(response.Data.DataFechamento, Is.EqualTo(request.DataFechamento));
                Assert.That(response.Data.DataVencimento, Is.EqualTo(request.DataVencimento));
                Assert.That(response.Data.Limite, Is.EqualTo(request.Limite));
                _cartaoCreditoRepositorioMock.Verify(r => r.BeginTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.CommitTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.RollbackTransactionAsync(), Times.Never);
                _dataFechamentoCartaoRepositorioMock.Verify(r => r.Insert(It.IsAny<Entity.DataFechamentoCartaoCredito>()), Times.Never);
            });
        }

        [Test]
        public async Task Handle_SuccessfulUpdate_WithDateChange()
        {
            var cartaoMock = new Entity.CartaoCredito
            {
                Id = Guid.NewGuid(),
                Nome = "Cartão Teste",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };

            var request = new AtualizarCartaoRequest
            {
                Id = cartaoMock.Id,
                Nome = "Cartão Atualizado",
                DataFechamento = DateTime.UtcNow.AddDays(1), // Mudança de data
                DataVencimento = DateTime.UtcNow.AddDays(11), // Mudança de data
                Limite = 10000
            };

            var dataFechamentoMock = new DataFechamentoCartaoCredito()
            {
                CartaoCreditoId = cartaoMock.Id,
                DataFechamentoAnterior = cartaoMock.DataFechamento,
                DataAlteracao = DateTime.UtcNow,
                DataVencimentoAnterior = cartaoMock.DataVencimento
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                                         .ReturnsAsync(cartaoMock);
            _cartaoCreditoRepositorioMock.Setup(r => r.Update(It.IsAny<Entity.CartaoCredito>()))
                                         .ReturnsAsync(cartaoMock);
            _dataFechamentoCartaoRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.DataFechamentoCartaoCredito>()))
                                                .ReturnsAsync(dataFechamentoMock);

            var response = await _atualizarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                Assert.That(response.Data.Id, Is.EqualTo(cartaoMock.Id));
                Assert.That(response.Data.Nome, Is.EqualTo(request.Nome));
                Assert.That(response.Data.DataFechamento, Is.EqualTo(request.DataFechamento));
                Assert.That(response.Data.DataVencimento, Is.EqualTo(request.DataVencimento));
                Assert.That(response.Data.Limite, Is.EqualTo(request.Limite));
                _cartaoCreditoRepositorioMock.Verify(r => r.BeginTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.CommitTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.RollbackTransactionAsync(), Times.Never);
                _dataFechamentoCartaoRepositorioMock.Verify(r => r.Insert(It.IsAny<Entity.DataFechamentoCartaoCredito>()), Times.Once);
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            var request = new AtualizarCartaoRequest
            {
                Id = Guid.NewGuid(),
                Nome = "Cartão Teste",
                DataFechamento = DateTime.UtcNow,
                DataVencimento = DateTime.UtcNow.AddDays(10),
                Limite = 5000
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<AtualizarCartaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _cartaoCreditoRepositorioMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                                         .ReturnsAsync(new Entity.CartaoCredito());
            _cartaoCreditoRepositorioMock.Setup(r => r.Update(It.IsAny<Entity.CartaoCredito>()))
                                         .ThrowsAsync(new Exception("Erro ao atualizar cartão"));

            var response = await _atualizarCartao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
                _cartaoCreditoRepositorioMock.Verify(r => r.BeginTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.RollbackTransactionAsync(), Times.Once);
                _cartaoCreditoRepositorioMock.Verify(r => r.CommitTransactionAsync(), Times.Never);
            });
        }
    }
}
