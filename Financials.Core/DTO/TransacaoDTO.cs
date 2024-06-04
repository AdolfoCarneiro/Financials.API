using Financials.Core.Entity;
using Financials.Core.Enums;

namespace Financials.Core.DTO
{
    public class TransacaoDTO
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public Guid? ContaId { get; set; }
        public Guid CategoriaId { get; set; }
        public Guid? CartaoCreditoId { get; set; }
        public Guid? FaturaId { get; set; }
        public bool Recorrente { get; set; } = false;
        public FrequenciaRecorrencia FrequenciaRecorrencia { get; set; }
        public int TotalParcelas { get; set; } = 1;
        public int NumeroParcela { get; set; } = 1;
        public Guid UserId { get; set; }
    }
}
