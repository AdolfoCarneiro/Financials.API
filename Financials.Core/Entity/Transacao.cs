using Financials.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class Transacao
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public Guid? ContaId { get; set; }
        public virtual Conta Conta { get; set; }
        public Guid CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public Guid? CartaoCreditoId { get; set; }
        public virtual CartaoCredito CartaoCredito { get; set; }
        public Guid? FaturaId { get; set; }
        public virtual Fatura Fatura { get; set; }
        public bool Recorrente { get; set; } = false;
        public FrequenciaRecorrencia FrequenciaRecorrencia { get; set; }
        public int TotalParcelas { get; set; } = 1;
        public int NumeroParcela { get; set; } = 1;
    }
}

