using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Account
{
    public class LoginRequest : IRequest<ApplicationResponse<UserLoginResponse>>
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }
}
