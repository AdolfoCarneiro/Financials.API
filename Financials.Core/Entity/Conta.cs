using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class Conta
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoInicial { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; }
        public virtual ICollection<Transferencia> TransferenciasEnviadas { get; set; }
        public virtual ICollection<Transferencia> TransferenciasRecebidas { get; set; }
    }
}
