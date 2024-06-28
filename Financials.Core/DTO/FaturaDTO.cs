namespace Financials.Core.DTO
{
    public class FaturaDTO
    {
        public decimal Valor { get; set; }
        public IList<TransacaoDTO> Transacoes { get; set; }
        public DateTime Fechamento { get; set; }
        public DateTime Vencimento { get; set; }
        public CartaoCreditoDTO CartaoCredito { get; set; }
    }
}
