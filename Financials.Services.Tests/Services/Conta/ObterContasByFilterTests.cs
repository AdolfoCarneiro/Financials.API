using Financials.Core.Enums;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Features.Conta;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Tests.Services.Conta
{
    [TestFixture]
    [Category("UnitTests")]
    public class ObterContasByFilterTests
    {
        private GetContasByFilter _getContasByFilter;
        private Mock<IContaRespositorio> _contaRepositorioMock;

        [SetUp]
        public void SetUp()
        {
            _contaRepositorioMock = new Mock<IContaRespositorio>();
            _getContasByFilter = new GetContasByFilter(_contaRepositorioMock.Object);
        }

        [Test]
        public async Task Handle_NoFilter_ReturnsAllAccounts()
        {
            var contasMock = new List<Entity.Conta>
            {
                new() { Id = Guid.NewGuid(), Nome = "Conta1", Tipo = TipoConta.Corrente },
                new() { Id = Guid.NewGuid(), Nome = "Conta2", Tipo = TipoConta.Poupanca }
            };
            _contaRepositorioMock.Setup(r => r.GetByExpression(It.IsAny<Expression<Func<Entity.Conta, bool>>>()))
                                 .Returns(contasMock.AsQueryable());

            var request = new GetContasByFilterRequest();

            var response = await _getContasByFilter.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                response.Data.Should().BeEquivalentTo(contasMock.Select(c => c.ToMapper()));
            });
        }

        [Test]
        public async Task Handle_FilterByName_ReturnsFilteredAccounts()
        {
            var contasMock = new List<Entity.Conta>
            {
                new() { Id = Guid.NewGuid(), Nome = "Conta1", Tipo = TipoConta.Corrente },
            };
            _contaRepositorioMock.Setup(r => r.GetByExpression(It.IsAny<Expression<Func<Entity.Conta, bool>>>()))
                                 .Returns(contasMock.AsQueryable());

            var request = new GetContasByFilterRequest { Filtro = "Conta1" };

            var response = await _getContasByFilter.Handle(request, CancellationToken.None);

            var expectedResult = contasMock.Where(c => c.Nome.Equals("Conta1", StringComparison.CurrentCultureIgnoreCase)).Select(c => c.ToMapper());
            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                response.Data.Should().BeEquivalentTo(expectedResult);
            });
        }

        [Test]
        public async Task Handle_FilterByType_ReturnsFilteredAccounts()
        {
            var contasMock = new List<Entity.Conta>
            {
                new() { Id = Guid.NewGuid(), Nome = "Conta1", Tipo = TipoConta.Corrente },
            };
            _contaRepositorioMock.Setup(r => r.GetByExpression(It.IsAny<Expression<Func<Entity.Conta, bool>>>()))
                                 .Returns(contasMock.AsQueryable());

            var request = new GetContasByFilterRequest { TipoConta = TipoConta.Corrente };

            var response = await _getContasByFilter.Handle(request, CancellationToken.None);

            var expectedResult = contasMock.Where(c => c.Tipo == TipoConta.Corrente).Select(c => c.ToMapper());
            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                response.Data.Should().BeEquivalentTo(expectedResult);
            });
        }

        [Test]
        public async Task Handle_FilterByNameAndType_ReturnsFilteredAccounts()
        {
            var contasMock = new List<Entity.Conta>
            {
                new() { Id = Guid.NewGuid(), Nome = "Conta1", Tipo = TipoConta.Corrente }
            };
            _contaRepositorioMock.Setup(r => r.GetByExpression(It.Is<Expression<Func<Entity.Conta, bool>>>(expr => expr.Compile()(contasMock.First()))))
                                 .Returns(contasMock.AsQueryable());

            var request = new GetContasByFilterRequest { Filtro = "Conta1", TipoConta = TipoConta.Corrente };

            var response = await _getContasByFilter.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                response.Data.Should().BeEquivalentTo(contasMock.Select(c => c.ToMapper()));
            });
        }

        [Test]
        public async Task Handle_NoAccountsFound_ReturnsEmptyList()
        {
            var contasMock = new List<Entity.Conta>();
            _contaRepositorioMock.Setup(r => r.GetByExpression(It.IsAny<Expression<Func<Entity.Conta, bool>>>()))
                                 .Returns(contasMock.AsQueryable());

            var request = new GetContasByFilterRequest();

            var response = await _getContasByFilter.Handle(request, CancellationToken.None);

            Assert.Multiple(() =>
            {
                Assert.That(response.Valid, Is.True);
                Assert.That(response.Error, Is.Null);
                Assert.That(response.Data, Is.Not.Null);
                Assert.That(response.Data, Is.Empty);
            });
        }

        [Test]
        public async Task Handle_ExceptionThrown_ReturnsErrorResponse()
        {

            _contaRepositorioMock.Setup(r => r.GetByExpression(It.IsAny<Expression<Func<Entity.Conta, bool>>>()))
                                 .Throws(new Exception("Erro de teste"));

            var request = new GetContasByFilterRequest();

            var response = await _getContasByFilter.Handle(request, CancellationToken.None);

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
