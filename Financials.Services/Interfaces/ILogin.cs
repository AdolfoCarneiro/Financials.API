using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;

namespace Financials.Services.Interfaces
{
    public interface ILogin
    {
        Task<ApplicationResponse<UserLoginResponse>> Run(LoginRequest request);
    }
}