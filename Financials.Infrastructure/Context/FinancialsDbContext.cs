using Financials.Core.Entity;
using Financials.Core.Interfaces;
using Financials.Infrastructure.HttpService;
using Financials.Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Financials.Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class FinancialsDbContext(DbContextOptions<FinancialsDbContext> options,IUserContext userContext, UserSaveChangesInterceptor saveChangesInterceptor) :  IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IUserContext _userContext = userContext;
        private readonly UserSaveChangesInterceptor _saveChangesInterceptor = saveChangesInterceptor;
        public DbSet<CartaoCredito> CartaoCredito { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Conta> Conta { get; set; }
        public DbSet<Transacao> Transacao { get; set; }
        public DbSet<Transferencia> Transferencia { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IUserOwnedResource).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(IUserOwnedResource.UserId));
                    var methodInfo = typeof(FinancialsDbContext).GetMethod(nameof(GetCurrentUserId), BindingFlags.NonPublic | BindingFlags.Instance);
                    var currentUserId = Expression.Call(Expression.Constant(this), methodInfo);
                    var emptyGuid = Expression.Constant(Guid.Empty);

                    var body = Expression.OrElse(
                        Expression.Equal(property, currentUserId),
                        Expression.Equal(property, emptyGuid)
                    );

                    var filter = Expression.Lambda(body, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }

            modelBuilder.Entity<CartaoCredito>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.CartaoCredito)
                .HasForeignKey(t => t.CartaoCreditoId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.Categoria)
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Conta>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.Conta)
                .HasForeignKey(t => t.ContaId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Conta>()
                .HasMany(c => c.TransferenciasEnviadas)
                .WithOne(t => t.ContaOrigem)
                .HasForeignKey(t => t.ContaOrigemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Conta>()
                .HasMany(c => c.TransferenciasRecebidas)
                .WithOne(t => t.ContaDestino)
                .HasForeignKey(t => t.ContaDestinoId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_saveChangesInterceptor);
            base.OnConfiguring(optionsBuilder);
        }
        private Guid GetCurrentUserId()
        {
            return _userContext.GetUserId();
        }
    }
}
