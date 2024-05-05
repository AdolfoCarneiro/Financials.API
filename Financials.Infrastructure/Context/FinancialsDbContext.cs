using Financials.Core.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;

namespace Financials.Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class FinancialsDbContext(DbContextOptions<FinancialsDbContext> options) :  IdentityDbContext<ApplicationUser>(options)
    {

        public DbSet<CartaoCredito> CartaoCredito { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Conta> Conta { get; set; }
        public DbSet<Fatura> Fatura { get; set; }
        public DbSet<Transacao> Transacao { get; set; }
        public DbSet<Transferencia> Transferencia { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
    }
}
