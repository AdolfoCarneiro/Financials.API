using Financials.Core.Entity;
using Financials.Core.VO;
using MediatR;

namespace Financials.Services.RequestsResponses.Account
{
    public class GenerateTokenRequest : IRequest<TokenVO>
    {
        public ApplicationUser User { get; set; }
    }
}
