using Financials.Core.Entity;
using Microsoft.AspNetCore.Identity;

namespace Financials.Services.Services.Account
{
    public class CriarUsuario
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CriarUsuario(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


    }
}
