using Financials.Core.Entity;
using Financials.Services.Interfaces.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using Microsoft.AspNetCore.Identity;

namespace Financials.Services.Features.Account
{
    public class CriarUsuario : ICriarUsuario
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CriarUsuario(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationResponse<UsuarioResponse>> Run(UsuarioRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
