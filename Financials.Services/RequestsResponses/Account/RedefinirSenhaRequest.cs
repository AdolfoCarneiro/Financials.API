using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Account
{
    public class RedefinirSenhaRequest : IRequest<ApplicationResponse<object>>
    {
        public string NovaSenha { get; set; }
        public Guid UsuarioId { get; set; }
        public string Token { get; set; }
    }
}
