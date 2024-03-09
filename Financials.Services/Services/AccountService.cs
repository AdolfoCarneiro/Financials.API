using Financials.Core.Entity;
using Financials.Infrastructure.Configuraton;
using Financials.Infrastructure.Repositorio;
using Financials.Services.RequestsResponses.Account;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Financials.Services.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFInancialsRepositorio _repositorio;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<UsuarioRequest> _usuarioValidator;
        private readonly JWTConfiguration _jwtConfiguration;
    }
}
