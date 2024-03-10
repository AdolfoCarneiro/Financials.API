using Financials.Core.Entity;
using Financials.Infrastructure.Configuraton;
using Financials.Infrastructure.Repositorio;
using Financials.Services.Interfaces;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Financials.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IFInancialsRepositorio _repositorio;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<UsuarioRequest> _usuarioValidator;
        private readonly JWTConfiguration _jwtConfiguration;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IFInancialsRepositorio repositorio,
            SignInManager<ApplicationUser> signInManager,
            IValidator<LoginRequest> loginValidator,
            IValidator<UsuarioRequest> usuarioValidator,
            IOptions<JWTConfiguration> option)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._repositorio = repositorio;
            this._signInManager = signInManager;
            this._loginValidator = loginValidator;
            this._usuarioValidator = usuarioValidator;
            this._jwtConfiguration = option.Value;
        }

        public async Task<ApplicationResponse<UserLoginResponse>> Autenticar(LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
