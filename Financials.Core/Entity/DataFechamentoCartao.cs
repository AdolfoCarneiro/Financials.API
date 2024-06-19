using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class DataFechamentoCartaoCredito
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DataVencimentoAnterior { get; set; }
        public Guid DataFechamentoAnterior { get; set; }
        public DateTime DataAlteracao { get; set; }
        public Guid CartaoCreditoId { get; set; }
        public virtual CartaoCredito CartaoCredito { get; set; }
    }
}
