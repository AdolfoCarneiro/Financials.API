using Financials.Core.DTO;
using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Conta
{
    public class GetContaRequest : IRequest<ApplicationResponse<ContaDTO>>
    {
        public Guid ContaId { get; set; }
    }
}
