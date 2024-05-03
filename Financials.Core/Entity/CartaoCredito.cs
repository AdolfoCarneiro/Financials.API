using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Financials.Core.Entity
{
    [ExcludeFromCodeCoverage]
    public class CartaoCredito
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Limite { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; }
        public virtual ICollection<Fatura> Faturas { get; set; }
    }
}
