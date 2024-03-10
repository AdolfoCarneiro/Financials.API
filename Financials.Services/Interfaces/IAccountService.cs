using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;

namespace Financials.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationResponse<UserLoginResponse>> Autenticar(LoginRequest request);
    }
}