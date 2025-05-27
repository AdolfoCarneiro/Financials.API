using Financials.Core.Enums;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.Transacao;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Transacao;
using FluentValidation.Results;
using Moq;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Transacao
{
    [TestFixture]
    [Category("UnitTests")]
    public class RegistrarTransacaoTests
    {
        private RegistrarTransacao _registrarTransacao;
        private Mock<IValidator<RegristrarTransacaoRequest>> _validatorMock;
        private Mock<ITransacaoRepositorio> _transacaoRepositorioMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [SetUp]
        public void SetUp()
        {
            _validatorMock = new Mock<IValidator<RegristrarTransacaoRequest>>();
            _transacaoRepositorioMock = new Mock<ITransacaoRepositorio>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _registrarTransacao = new RegistrarTransacao(
                _validatorMock.Object,
                _transacaoRepositorioMock.Object,
                _unitOfWorkMock.Object);
        }

        [Test]
        public async Task Handle_ValidationFails_ReturnsErrorResponse()
        {
            var request = new RegristrarTransacaoRequest();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegristrarTransacaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("campo", "erro de validação") }));

            var response = await _registrarTransacao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Data, Is.Null);
                _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Never);
                _unitOfWorkMock.Verify(r => r.RollbackAsync(), Times.Never);
            });
        }

        [Test]
        public async Task Handle_RecurringTransaction_SuccessfullyRegistered()
        {
            var request = new RegristrarTransacaoRequest
            {
                Recorrente = true,
                Data = DateTime.UtcNow,
                CategoriaId = Guid.NewGuid(),
                Descricao = "Teste",
                Valor = 100,
                Tipo = TipoTransacao.Receita
            };
            var transacaoMock = new Entity.Transacao { Id = Guid.NewGuid() };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegristrarTransacaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _transacaoRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.Transacao>()))
                                     .ReturnsAsync(transacaoMock);

            var response = await _registrarTransacao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once);
                _unitOfWorkMock.Verify(r => r.RollbackAsync(), Times.Never);
            });
        }

        [Test]
        public async Task Handle_RecurringTransactionWithDifferentFrequencies_SuccessfullyRegistered()
        {
            var frequencies = new[]
            {
                FrequenciaRecorrencia.Diaria,
                FrequenciaRecorrencia.Semanal,
                FrequenciaRecorrencia.Mensal,
                FrequenciaRecorrencia.Anual
            };

            var interation = 0;
            foreach (var frequency in frequencies)
            {
                interation++;
                var request = new RegristrarTransacaoRequest
                {
                    Recorrente = true,
                    Data = DateTime.UtcNow,
                    CategoriaId = Guid.NewGuid(),
                    Descricao = "Teste",
                    Valor = 100,
                    Tipo = TipoTransacao.Receita,
                    FrequenciaRecorrencia = frequency
                };
                var transacaoMock = new Entity.Transacao { Id = Guid.NewGuid() };

                _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegristrarTransacaoRequest>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new ValidationResult());
                _transacaoRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.Transacao>()))
                                         .ReturnsAsync(transacaoMock);

                var response = await _registrarTransacao.Handle(request, CancellationToken.None);

                Assert.Multiple(() =>
                {
                    Assert.That(response.Valid, Is.True);
                    Assert.That(response.Error, Is.Null);
                    Assert.That(response.Data, Is.Not.Null);
   
                    _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Exactly(interation));
                    _unitOfWorkMock.Verify(r => r.RollbackAsync(), Times.Never);
                });

                _transacaoRepositorioMock.Invocations.Clear();
            }
        }

        [Test]
        public async Task Handle_InstallmentTransaction_SuccessfullyRegistered()
        {
            var request = new RegristrarTransacaoRequest
            {
                Recorrente = false,
                Data = DateTime.UtcNow,
                CategoriaId = Guid.NewGuid(),
                Descricao = "Compra",
                Valor = 300,
                TotalParcelas = 3,
                Tipo = TipoTransacao.Despesa,
                FrequenciaRecorrencia = FrequenciaRecorrencia.Mensal
            };
            var transacaoMock = new Entity.Transacao { Id = Guid.NewGuid() };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegristrarTransacaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _transacaoRepositorioMock.Setup(r => r.Insert(It.IsAny<List<Entity.Transacao>>()))
                                     .Returns(Task.CompletedTask);

            var response = await _registrarTransacao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once);
                _unitOfWorkMock.Verify(r => r.RollbackAsync(), Times.Never);
            });
        }

        [Test]
        public async Task Handle_InstallmentTransaction_CorrectlyDistributesAmounts()
        {
            var request = new RegristrarTransacaoRequest
            {
                Recorrente = false,
                Data = DateTime.UtcNow,
                CategoriaId = Guid.NewGuid(),
                Descricao = "Compra",
                Valor = 100,
                TotalParcelas = 3,
                Tipo = TipoTransacao.Despesa,
                FrequenciaRecorrencia = FrequenciaRecorrencia.Mensal
            };
            var transacoesCapturadas = new List<Entity.Transacao>();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegristrarTransacaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _transacaoRepositorioMock.Setup(r => r.Insert(It.IsAny<List<Entity.Transacao>>()))
                                     .Callback<List<Entity.Transacao>>(transacoes => transacoesCapturadas.AddRange(transacoes))
                                     .Returns(Task.CompletedTask);

            var response = await _registrarTransacao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once);
                _unitOfWorkMock.Verify(r => r.RollbackAsync(), Times.Never);

                Assert.That(transacoesCapturadas.Count, Is.EqualTo(3));
                Assert.That(transacoesCapturadas[0].Valor, Is.EqualTo(33.33m)); // Valor arredondado
                Assert.That(transacoesCapturadas[1].Valor, Is.EqualTo(33.33m)); // Valor arredondado
                Assert.That(transacoesCapturadas[2].Valor, Is.EqualTo(33.34m)); // Valor ajustado na última parcela
            });
        }

        [Test]
        public async Task Handle_TransactionRollbackOnCommitFailure()
        {
            var request = new RegristrarTransacaoRequest
            {
                Recorrente = true,
                Data = DateTime.UtcNow,
                CategoriaId = Guid.NewGuid(),
                Descricao = "Teste",
                Valor = 100,
                Tipo = TipoTransacao.Receita
            };
            var transacaoMock = new Entity.Transacao { Id = Guid.NewGuid() };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegristrarTransacaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _transacaoRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.Transacao>()))
                                     .ReturnsAsync(transacaoMock);
            _unitOfWorkMock.Setup(r => r.SaveChangesAsync())
                                     .ThrowsAsync(new Exception("Erro ao persistir alterações no banco"));

            var response = await _registrarTransacao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
                _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Once);
                _unitOfWorkMock.Verify(r => r.RollbackAsync(), Times.Once);
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {
            var request = new RegristrarTransacaoRequest();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<RegristrarTransacaoRequest>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _transacaoRepositorioMock.Setup(r => r.Insert(It.IsAny<List<Entity.Transacao>>()))
                                     .ThrowsAsync(new Exception("Erro de teste"));

            var response = await _registrarTransacao.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.False);
                Assert.That(response.Data, Is.Null);
                Assert.That(response.Error, Is.Not.Null);
                Assert.That(response.Error.Type, Is.EqualTo(ResponseErrorType.InternalError));
                _unitOfWorkMock.Verify(r => r.SaveChangesAsync(), Times.Never);
                _unitOfWorkMock.Verify(r => r.RollbackAsync(), Times.Once);
            });
        }
    }
}
