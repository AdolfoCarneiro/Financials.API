using Financials.Core.DTO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Transacao;
using FluentValidation;
using MediatR;

namespace Financials.Services.Features.Transacao
{
    public class RegistrarTransacao(
           IValidator<RegristrarTransacaoRequest> validator,
           ITransacaoRepositorio transacaoRepositorio
        )
        : IRequestHandler<RegristrarTransacaoRequest, ApplicationResponse<TransacaoDTO>>
    {
        private readonly IValidator<RegristrarTransacaoRequest> _validator = validator;
        private readonly ITransacaoRepositorio _transacaoRepositorio = transacaoRepositorio;

        public async Task<ApplicationResponse<TransacaoDTO>> Handle(RegristrarTransacaoRequest request, CancellationToken cancellationToken)
        {
            var response = new ApplicationResponse<TransacaoDTO>();
            try
            {
                var validacao = await _validator.ValidateAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {

                response.AddError(ex, "Erro ao registrar a transação");
            }
            return response;
        }

    }
}
