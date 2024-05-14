using Financials.Core.DTO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using FluentValidation;
using MediatR;

namespace Financials.Services.Features.Conta
{
    public class AtualizarConta(
        IContaRespositorio contaRespositorio, IValidator<AtualizarContaRequest> validator
        ) : IRequestHandler<AtualizarContaRequest, ApplicationResponse<ContaDTO>>
    {
        private readonly IContaRespositorio _contaRespositorio = contaRespositorio;
        private readonly IValidator<AtualizarContaRequest> _validator = validator;
        public async Task<ApplicationResponse<ContaDTO>> Handle(AtualizarContaRequest request, CancellationToken cancellationToken = default)
        {
            var response = new ApplicationResponse<ContaDTO>();
            try
            {
                var validacao = await _validator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                var conta = await _contaRespositorio.GetById(request.Id);

                if (conta is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Conta não encontrada");
                }

                conta.Tipo = request.Tipo;
                conta.Nome = request.Nome;
                conta.SaldoInicial = request.SaldoInicial;

                await _contaRespositorio.Update(conta);

                response.AddData(conta.ToMapper());
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao atualizar a conta");
            }
            return response;


        }
    }
}
