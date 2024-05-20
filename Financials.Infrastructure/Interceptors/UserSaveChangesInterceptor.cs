using Financials.Core.Interfaces;
using Financials.Infrastructure.HttpService;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Financials.Infrastructure.Interceptors
{
    public class UserSaveChangesInterceptor(IUserContext userContext) : SaveChangesInterceptor
    {
        private readonly IUserContext _userContext = userContext;

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            SetUserId(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            SetUserId(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void SetUserId(DbContext context)
        {
            var userId = _userContext.GetUserId();

            foreach (var entry in context.ChangeTracker.Entries<IUserOwnedResource>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.UserId = userId;
                }
            }
        }
    }
}