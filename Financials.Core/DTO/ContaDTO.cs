using Financials.Core.Enums;

namespace Financials.Core.DTO
{
    public class ContaDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoInicial { get; set; }
        public TipoConta Tipo { get; set; }
    }
}
