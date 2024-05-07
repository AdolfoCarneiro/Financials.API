using Financials.Core.Enums;

namespace Financials.Services.RequestsResponses.Conta
{
    public class ContaRequest
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoInicial { get; set; }
        public TipoConta Tipo { get; set; }
    }
}
