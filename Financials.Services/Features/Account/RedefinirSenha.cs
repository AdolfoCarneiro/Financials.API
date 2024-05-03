using Financials.Core.Entity;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Financials.Services.Features.Account
{
    public class RecuperacaoSenha
    {
        private readonly IValidator<RedefinirSenhaRequest> _validator;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecuperacaoSenha(IValidator<RedefinirSenhaRequest> validator, UserManager<ApplicationUser> userManager)
        {
            _validator = validator;
            _userManager = userManager;
        }

        public async Task<ApplicationResponse<object>> Run(RedefinirSenhaRequest request)
        {
            var response = new ApplicationResponse<object>();
            try
            {
                var validacao = await _validator.ValidateAsync(request);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                byte[] tokenBytes = WebEncoders.Base64UrlDecode(request.Token);
                string tokenDecodificado = Encoding.UTF8.GetString(tokenBytes);

                var usuario = await _userManager.FindByIdAsync(request.UsuarioId.ToString());
                if (usuario is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Usuario não encontrado");
                    return response;
                }

                var result = await _userManager.ResetPasswordAsync(usuario, tokenDecodificado,request.NovaSenha);
                if (!result.Succeeded)
                {
                    response.AddError(ResponseErrorType.InternalError, "Erro ao redefinir senha do usuário");
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.AddError(ex,"Erro ao redefinir senha");
            }
            return response;
        }
    }
}
