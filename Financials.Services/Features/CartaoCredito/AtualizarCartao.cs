using Financials.Core.DTO;
using Financials.Core.Entity;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Cartao;
using FluentValidation;
using MediatR;

namespace Financials.Services.Features.CartaoCredito
{
    public class AtualizarCartao(
            ICartaoCreditoRepositorio cartaoCreditoRepositorio,
            IDataFechamentoCartaoRepositorio dataFechamentoCartaoRepositorio,
            IValidator<AtualizarCartaoRequest> validator,
            IUnitOfWork unitOfWork
        )
        : IRequestHandler<AtualizarCartaoRequest, ApplicationResponse<CartaoCreditoDTO>>
    {
        private readonly ICartaoCreditoRepositorio _cartaoCreditoRepositorio = cartaoCreditoRepositorio;
        private readonly IDataFechamentoCartaoRepositorio _dataFechamentoCartaoRepositorio = dataFechamentoCartaoRepositorio;
        private readonly IValidator<AtualizarCartaoRequest> _validator = validator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ApplicationResponse<CartaoCreditoDTO>> Handle(AtualizarCartaoRequest request, CancellationToken cancellationToken)
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

                var cartao = await _cartaoCreditoRepositorio.GetById(request.Id);

                if (cartao is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Cartão informado não encontrado");
                    return response;
                }

                if (cartao.DataFechamento.Date != request.DataFechamento.Date || cartao.DataVencimento.Date != request.DataVencimento.Date)
                {
                    var historicoDataFechamentoCartao = new DataFechamentoCartaoCredito()
                    {
                        CartaoCreditoId = cartao.Id,
                        DataAlteracao = DateTime.Now.Date,
                        DataFechamentoAnterior = cartao.DataFechamento,
                        DataVencimentoAnterior = cartao.DataVencimento,
                    };

                    await _dataFechamentoCartaoRepositorio.Insert(historicoDataFechamentoCartao);
                }

                cartao.DataFechamento = request.DataFechamento;
                cartao.DataVencimento = request.DataVencimento;
                cartao.Limite = request.Limite;
                cartao.Nome = request.Nome;

                _cartaoCreditoRepositorio.Update(cartao);

                await _unitOfWork.SaveChangesAsync();

                response.AddData(cartao.ToMapper());
                return response;
            }
            catch (Exception ex)
            {
                response.AddError(ex);
                await _unitOfWork.RollbackAsync();
            }
            return response;
        }
    }
}
