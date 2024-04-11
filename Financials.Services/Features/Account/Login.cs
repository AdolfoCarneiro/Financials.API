using Financials.Core.Entity;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Financials.Services.Features.Account
{
    public class Login(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IValidator<LoginRequest> validator,
        GerarTokens gerarTokens)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IValidator<LoginRequest> _validator = validator;
        private readonly GerarTokens _gerarTokens = gerarTokens;

        public async Task<ApplicationResponse<UserLoginResponse>> Run(LoginRequest request)
        {
            var response = new ApplicationResponse<UserLoginResponse>();
            try
            {
                var validacao = await _validator.ValidateAsync(request);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                var user = await _userManager.FindByEmailAsync(request.Usuario);
                if (user is null)
                {
                    response.AddError(ResponseErrorType.InternalError, "Erro ao realizar login");
                }

                var loginResult = await _signInManager.PasswordSignInAsync(user, request.Senha, false, false);
                if (!loginResult.Succeeded)
                {
                    response.AddError(ResponseErrorType.InternalError, "Erro ao realizar login");
                }

                var token = await _gerarTokens.Run(user);

                UserLoginResponse responseData = new()
                {
                    Token = token
                };
                response.AddData(responseData);
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao realizar login");
            }
            return response;
        }

        
    }
}
