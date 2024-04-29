using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Financials.Services.Features.Account
{
    public class RecuperacaoSenha
    {
        private readonly IValidator<RedefinirSenhaRequest> _validator;
        public RecuperacaoSenha(IValidator<RedefinirSenhaRequest> validator)
        {
            _validator = validator;
        }

        public async Task<ApplicationResponse<object>> RedefinirSenha(RedefinirSenhaRequest request)
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

            }
            catch (Exception ex)
            {
                response.AddError(ex,"Erro ao redefinir senha");
            }
            return response;
        }
    }
}
