using Financials.Core.DTO;
using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Fatura
{
    public class ObterFaturaRequest : IRequest<ApplicationResponse<FaturaDTO>>
    {
        public DateTime DataReferencia { get; set; }
        public Guid CartaoId { get; set; }
    }
}
