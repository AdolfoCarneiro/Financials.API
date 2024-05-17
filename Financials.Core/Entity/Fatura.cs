using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Financials.Core.Interfaces;

namespace Financials.Core.Entity
{
    [ExcludeFromCodeCoverage]
    public class Fatura : IUserOwnedResource
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public Guid CartaoCreditoId { get; set; }
        public virtual CartaoCredito CartaoCredito { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; }
        public Guid UserId { get; set; }
    }
}
