using Financials.Core.DTO;
using Financials.Core.Entity;
using Financials.Infrastructure.ServicosExternos;
using Financials.Services.RequestsResponses.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Financials.Services.Features.Account
{
    public class RedefinirSenha
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ServicoEmail _servicoEmail;
        public RedefinirSenha(UserManager<ApplicationUser> userManager, ServicoEmail servicoEmail) 
        {
            _userManager = userManager;
            _servicoEmail = servicoEmail;
        }

        public async Task<ApplicationResponse<object>> Run(string email)
        {
            var response = new ApplicationResponse<object>();
            try
            {
                var usuario = await _userManager.FindByEmailAsync(email);
                if (usuario is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Usuario não encontrado");
                    return response;
                }
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                if (String.IsNullOrEmpty(passwordResetToken))
                {
                    response.AddError(ResponseErrorType.InternalError, "Falha ao gerar token de recuperação de senha.");
                }
                passwordResetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordResetToken));

                var nomeUsuario = usuario.Nome;

                EmailDTO emailDto = new()
                {
                    Body = $"http://localhost:3000/acccount/redefinirSenha?token={passwordResetToken}&userId={usuario.Id}",
                    Destinatario = email,
                };
                var enviarInsightResponse = await _servicoEmail.EnviarEmail(emailDto);
                if (!enviarInsightResponse)
                {
                    response.AddError(ResponseErrorType.InternalError,"Erro ao enviar email de redefinição de senha");
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex,"Erro ao enviar email de redefinição de senha");
            }
            return response;
        }
    }
}
