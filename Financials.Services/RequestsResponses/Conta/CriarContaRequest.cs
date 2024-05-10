using Financials.Core.DTO;
using Financials.Core.Enums;
using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Conta
{
    public class CriarContaRequest : IRequest<ApplicationResponse<ContaDTO>>
    {
        public string Nome { get; set; }
        public decimal SaldoInicial { get; set; }
        public TipoConta Tipo { get; set; }
    }
}
