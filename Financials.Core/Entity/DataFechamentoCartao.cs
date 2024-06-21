using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class DataFechamentoCartaoCredito
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime DataVencimentoAnterior { get; set; }
        public DateTime DataFechamentoAnterior { get; set; }
        public DateTime DataAlteracao { get; set; }
        public Guid CartaoCreditoId { get; set; }
        public virtual CartaoCredito CartaoCredito { get; set; }
    }
}
