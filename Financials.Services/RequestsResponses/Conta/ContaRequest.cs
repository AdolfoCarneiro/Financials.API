using Financials.Core.Enums;
using MediatR;

namespace Financials.Services.RequestsResponses.Conta
{
    public class ContaRequest : IRequest<ContaResponse>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoInicial { get; set; }
        public TipoConta Tipo { get; set; }
    }
}
