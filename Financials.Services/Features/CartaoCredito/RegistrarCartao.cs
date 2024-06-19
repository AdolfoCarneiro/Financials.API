using Financials.Core.DTO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Cartao;
using FluentValidation;
using MediatR;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Features.CartaoCredito
{
    public class RegistrarCartao(
        IValidator<RegistrarCartaoRequest> validator,
        ICartaoCreditoRepositorio cartaoCreditoRepositorio
        ) : IRequestHandler<RegistrarCartaoRequest, ApplicationResponse<CartaoCreditoDTO>>
    {
        private readonly IValidator<RegistrarCartaoRequest> _validator = validator;
        private readonly ICartaoCreditoRepositorio _cartaoCreditoRepositorio = cartaoCreditoRepositorio;
        public async Task<ApplicationResponse<CartaoCreditoDTO>> Handle(RegistrarCartaoRequest request, CancellationToken cancellationToken)
        {
            var response = new ApplicationResponse<CartaoCreditoDTO>();
            try
            {
                var validacao = await _validator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                var cartao = new Entity.CartaoCredito()
                {
                    Nome = request.Nome,
                    DataFechamento = request.DataFechamento,
                    DataVencimento = request.DataVencimento,
                    Limite = request.Limite,         
                };

                cartao = await _cartaoCreditoRepositorio.Insert(cartao);

                var cartaoDto = cartao.ToMapper();

                response.AddData(cartaoDto);
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao registrar cartão");
            }
            return response;
        }
    }
}
