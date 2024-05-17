﻿using Financials.Core.Enums;
using Financials.Infrastructure.HttpService;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.Conta;
using Financials.Services.RequestsResponses.Conta;
using FluentValidation.Results;
using Moq;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Conta
{
    [TestFixture]
    [Category("UnitTests")]
    public class CriarContaTests
    {
        private CriarConta _criarConta;
        private Mock<IContaRespositorio> _contaRepositorioMock;
        private Mock<IValidator<CriarContaRequest>> _validatorMock;
        private Mock<IUserContext> _userContextMock;

        [SetUp]
        public void SetUp()
        {
            _contaRepositorioMock = new Mock<IContaRespositorio>();
            _validatorMock = new Mock<IValidator<CriarContaRequest>>();
            _userContextMock = new Mock<IUserContext>();
            _criarConta = new CriarConta(_contaRepositorioMock.Object, _validatorMock.Object,_userContextMock.Object);
        }

        [Test]
        public async Task Handle_Should_Return_Errors_If_Validation_Fails()
        {
            var request = new CriarContaRequest();
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("Nome", "Nome é obrigatório") };
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult(validationFailures));

            var result = await _criarConta.Handle(request, CancellationToken.None);

            Assert.That(result.Valid, Is.False);
            Assert.That(result.Error, Is.Not.Null);
            Assert.That(result.Error.CustomMessage, Is.EqualTo("Nome é obrigatório"));
        }

        [Test]
        public async Task Handle_Should_Create_Account_And_Return_Correct_Data_If_Validation_Succeeds()
        {
            var request = new CriarContaRequest
            {
                Nome = "Conta Teste",
                SaldoInicial = 1000,
                Tipo = TipoConta.Corrente
            };
            var conta = new Entity.Conta
            {
                Id = Guid.NewGuid(),
                Nome = "Conta Teste",
                SaldoInicial = 1000,
                Tipo = TipoConta.Corrente
            };
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.Conta>()))
                                 .ReturnsAsync(conta);

            var result = await _criarConta.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.True);
                Assert.That(result.Data.Nome, Is.EqualTo("Conta Teste"));
                Assert.That(result.Data.SaldoInicial, Is.EqualTo(1000));
                Assert.That(result.Data.Tipo, Is.EqualTo(TipoConta.Corrente));
                Assert.That(result.Data.Id, Is.Not.EqualTo(Guid.Empty));
            });
        }

        [Test]
        public async Task Handle_Should_Return_Error_If_Exception_Is_Thrown()
        {
            var request = new CriarContaRequest();
            _validatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());
            _contaRepositorioMock.Setup(r => r.Insert(It.IsAny<Entity.Conta>()))
                                 .ThrowsAsync(new Exception("Erro ao acessar o banco de dados"));

            var result = await _criarConta.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error, Is.Not.Null);
                Assert.That(result.Error.CustomMessage, Is.EqualTo("Erro ao criar a conta"));
            });

        }
    }
}
