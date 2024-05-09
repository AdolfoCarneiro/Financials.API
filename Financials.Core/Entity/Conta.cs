using Financials.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Financials.Core.Entity
{
    [ExcludeFromCodeCoverage]
    public class Conta
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoInicial { get; set; }
        public TipoConta Tipo { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; }
        public virtual ICollection<Transferencia> TransferenciasEnviadas { get; set; }
        public virtual ICollection<Transferencia> TransferenciasRecebidas { get; set; }
        [NotMapped]
        public decimal Saldo
        {
            get
            {
                var receitas = Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
                var despesas = Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);
                var transferenciasEnviadas = TransferenciasEnviadas.Sum(t => t.Valor);
                var transferenciasRecebidas = TransferenciasRecebidas.Sum(t => t.Valor);

                return SaldoInicial + ((receitas + transferenciasRecebidas) - (despesas + transferenciasEnviadas));
            }
        }
    }
}
