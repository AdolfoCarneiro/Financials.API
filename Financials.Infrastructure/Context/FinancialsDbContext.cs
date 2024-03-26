using Financials.Core.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Financials.Infrastructure.Context
{
    public class FinancialsDbContext:  IdentityDbContext<ApplicationUser>
    {
        public FinancialsDbContext(DbContextOptions<FinancialsDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

}
