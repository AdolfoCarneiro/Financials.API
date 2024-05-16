using Financials.Core.DTO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using FluentValidation;
using MediatR;

namespace Financials.Services.Features.Conta
{
    public class ObterConta(IContaRespositorio contaRespositorio, IValidator<GetContaRequest> validator) : IRequestHandler<GetContaRequest, ApplicationResponse<ContaDTO>>
    {
        private readonly IContaRespositorio _contaRespositorio = contaRespositorio;
        private readonly IValidator<GetContaRequest> _validator = validator;
        public async Task<ApplicationResponse<ContaDTO>> Handle(GetContaRequest request, CancellationToken cancellationToken)
        {
            var response = new ApplicationResponse<ContaDTO>();
            try
            {
                var validacao = await _validator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                }
                var conta = await _contaRespositorio.GetById(request.ContaId);

                if (conta is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Conta não encontrada");
                    return response;
                }

                response.AddData(conta.ToMapper());
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao obter a conta");
            }
            return response;
        }
    }
}
