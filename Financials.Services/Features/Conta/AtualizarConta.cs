using Financials.Core.DTO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using Financials.Services.RequestsResponses.Conta.Validators;
using FluentValidation;
using MediatR;

namespace Financials.Services.Features.Conta
{
    public class AtualizarConta(
        IContaRespositorio contaRespositorio, IValidator<AtualizarContaRequestValidator> validator
        ) : IRequestHandler<AtualizarContaRequest, ApplicationResponse<ContaDTO>>
    {
        private readonly IContaRespositorio _contaRespositorio;
        private readonly IValidator<AtualizarContaRequestValidator> _validator;
        public async Task<ApplicationResponse<ContaDTO>> Handle(AtualizarContaRequest request, CancellationToken cancellationToken)
        {
            var response = new ApplicationResponse<ContaDTO>();
            try
            {

            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao atualizar a conta");
            }
            return response;


        }
    }
}
