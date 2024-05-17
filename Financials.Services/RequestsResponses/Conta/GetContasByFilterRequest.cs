using Financials.Core.DTO;
using Financials.Core.Enums;
using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Conta
{
    public class GetContasByFilterRequest : IRequest<ApplicationResponse<IEnumerable<ContaDTO>>>
    {
        public string Filtro { get; set; } = string.Empty;
        public TipoConta? TipoConta { get; set; } = null;

    }
}
