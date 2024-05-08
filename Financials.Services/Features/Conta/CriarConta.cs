using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses;
using FluentValidation;
using Entity = Financials.Core.Entity;
using Financials.Services.RequestsResponses.Conta;

namespace Financials.Services.Features.Conta
{
    public class CriarConta(
        IContaRespositorio contaRepositorio,
        IValidator<ContaRequest> validator
        )
    {
        private readonly IContaRespositorio _contaRepositorio = contaRepositorio;
        private readonly IValidator<ContaRequest> _validator = validator;

        public virtual async Task<ApplicationResponse<ContaResponse>> Run(ContaRequest request)
        {
            ApplicationResponse<ContaResponse> response = new();
            try
            {
                var validacao = await _validator.ValidateAsync(request);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                Entity.Conta conta = new()
                {

                };
            }
            catch (Exception ex)
            {
                response.AddError(ex,"Erro ao criar a conta");
            }
            return response;
        }

    }
}
