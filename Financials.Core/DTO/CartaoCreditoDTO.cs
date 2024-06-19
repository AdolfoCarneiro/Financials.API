namespace Financials.Core.DTO
{
    public class CartaoCreditoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Limite { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
