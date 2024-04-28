using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class Fatura
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public Guid CartaoCreditoId { get; set; }
        public virtual CartaoCredito CartaoCredito { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; }
    }
}
