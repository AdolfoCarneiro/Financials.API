using Financials.Core.Entity;
using Financials.Infrastructure.Context;
using Financials.Infrastructure.HttpService;
using Financials.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Financials.Infrastructure.Tests.Context
{
    [TestFixture]
    [Category("UnitTests")]
    [Category("IntegrationTests")]
    public class FinancialsDbContextTests
    {
        private Mock<IUserContext> _userContextMock;
        private UserSaveChangesInterceptor _saveChangesInterceptor;

        [SetUp]
        public void SetUp()
        {
            _userContextMock = new Mock<IUserContext>();
            _saveChangesInterceptor = new UserSaveChangesInterceptor(_userContextMock.Object);
        }

        [Test]
        public async Task SavingChanges_ShouldSetUserId()
        {
            var userId = Guid.NewGuid();
            _userContextMock.Setup(x => x.GetUserId()).Returns(userId);

            DbContextOptions<FinancialsDbContext> options = new DbContextOptionsBuilder<FinancialsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .AddInterceptors(_saveChangesInterceptor)
                .Options;

            using var context = new FinancialsDbContext(options, _userContextMock.Object, _saveChangesInterceptor);
            var data = new Transacao { Id = Guid.NewGuid() };

            context.Transacao.Add(data);
            await context.SaveChangesAsync();

            Assert.That(data.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void Query_ShouldFilterByUserIdOrGuidEmpty()
        {
            var userId = Guid.NewGuid();
            _userContextMock.Setup(x => x.GetUserId()).Returns(userId);

            DbContextOptions<FinancialsDbContext> options = new DbContextOptionsBuilder<FinancialsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new FinancialsDbContext(options, _userContextMock.Object, null);
            context.Transacao.Add(new Transacao { Id = Guid.NewGuid(), UserId = userId });
            context.Transacao.Add(new Transacao { Id = Guid.NewGuid(), UserId = Guid.NewGuid() });
            context.Transacao.Add(new Transacao { Id = Guid.NewGuid(), UserId = Guid.Empty });
            context.SaveChanges();

            var result = context.Transacao.ToList();

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(2));
                Assert.That(result.Any(r => r.UserId == userId), Is.True);
                Assert.That(result.Any(r => r.UserId == Guid.Empty), Is.True);
            });
        }
    }

}