using Financials.Core.DTO;
using Financials.Infrastructure.HttpService;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using FluentValidation;
using MediatR;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Features.Conta
{
    public class CriarConta(
        IContaRespositorio contaRepositorio,
        IValidator<CriarContaRequest> validator,
        IUserContext userContext
        ) : IRequestHandler<CriarContaRequest, ApplicationResponse<ContaDTO>>
    {
        private readonly IContaRespositorio _contaRepositorio = contaRepositorio;
        private readonly IValidator<CriarContaRequest> _validator = validator;
        private readonly IUserContext _userContext = userContext;

        public virtual async Task<ApplicationResponse<ContaDTO>> Handle(CriarContaRequest request, CancellationToken cancellationToken = default)
        {
            ApplicationResponse<ContaDTO> response = new();
            try
            {
                var validacao = await _validator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                Entity.Conta conta = new()
                {
                    Nome = request.Nome,
                    SaldoInicial = request.SaldoInicial,
                    Tipo = request.Tipo,
                    UserId = _userContext.GetUserId()
                };

                conta = await _contaRepositorio.Insert(conta);

                response.AddData(conta.ToMapper());
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao criar a conta");
            }
            return response;
        }

    }
}
