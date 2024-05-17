using Financials.Core.Entity;
using Financials.Core.Interfaces;
using Financials.Infrastructure.HttpService;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Financials.Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class FinancialsDbContext(DbContextOptions<FinancialsDbContext> options,IUserContext userContext) :  IdentityDbContext<ApplicationUser>(options)
    {
        private readonly IUserContext _userContext = userContext;
        public DbSet<CartaoCredito> CartaoCredito { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Conta> Conta { get; set; }
        public DbSet<Fatura> Fatura { get; set; }
        public DbSet<Transacao> Transacao { get; set; }
        public DbSet<Transferencia> Transferencia { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IUserOwnedResource).IsAssignableFrom(entityType.ClrType))
                {
                    var userIdProperty = entityType.FindProperty(nameof(IUserOwnedResource.UserId));
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, userIdProperty.PropertyInfo);
                    var methodInfo = typeof(FinancialsDbContext).GetMethod(nameof(GetCurrentUserId), BindingFlags.NonPublic | BindingFlags.Instance);
                    var body = Expression.Call(Expression.Constant(this), methodInfo);
                    var filter = Expression.Equal(property, body);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(filter, parameter));
                }
            }

            modelBuilder.Entity<CartaoCredito>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.CartaoCredito)
                .HasForeignKey(t => t.CartaoCreditoId)
                .IsRequired(false); 


            modelBuilder.Entity<Categoria>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.Categoria)
                .HasForeignKey(t => t.CategoriaId);

            modelBuilder.Entity<Conta>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.Conta)
                .HasForeignKey(t => t.ContaId)
                .IsRequired(false); 

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

            modelBuilder.Entity<Fatura>()
                .HasMany(f => f.Transacoes)
                .WithOne(t => t.Fatura)
                .HasForeignKey(t => t.FaturaId)
                .IsRequired(false); 

            modelBuilder.Entity<CartaoCredito>()
                .HasMany(c => c.Faturas)
                .WithOne(f => f.CartaoCredito)
                .HasForeignKey(t => t.CartaoCreditoId);
        }

        private Guid GetCurrentUserId()
        {
            return _userContext.GetUserId();
        }
    }
}
