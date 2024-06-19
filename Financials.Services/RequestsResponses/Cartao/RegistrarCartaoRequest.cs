using Financials.Core.DTO;
using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Cartao
{
    public class RegistrarCartaoRequest : IRequest<ApplicationResponse<CartaoCreditoDTO>>
    {
        public string Nome { get; set; }
        public decimal Limite { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
