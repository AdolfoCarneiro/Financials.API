using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;

namespace Financials.Services.Interfaces.Account
{
    public interface ICriarUsuario
    {
        Task<ApplicationResponse<UsuarioResponse>> Run(UsuarioRequest request);
    }
}
