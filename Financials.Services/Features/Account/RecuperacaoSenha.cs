using Financials.Services.RequestsResponses.Base;

namespace Financials.Services.Features.Account
{
    public class RedefinirSenha
    {
        public RedefinirSenha() 
        {
            
        }

        public async Task<ApplicationResponse<object>> EnviarEmailRedefinicaoSenha(string email)
        {
            var response = new ApplicationResponse<object>();
            try
            {

            }
            catch (Exception ex)
            {
                response.AddError(ex,"Erro ao enviar email de redefinição de senha");
            }
            return response;
        }
    }
}
