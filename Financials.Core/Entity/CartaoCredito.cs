using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class CartaoCredito
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Limite { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime DataVencimento { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; }
    }
}
