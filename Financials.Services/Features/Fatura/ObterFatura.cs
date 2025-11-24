using Financials.Core.DTO;
using Financials.Core.Entity;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Fatura;
using FluentValidation;
using MediatR;

namespace Financials.Services.Features.Fatura
{
    public class ObterFatura(
        IValidator<ObterFaturaRequest> validator,
        IDataFechamentoCartaoRepositorio dataFechamentoCartaoRepositorio,
        ICartaoCreditoRepositorio cartaoCreditoRepositorio,
        ITransacaoRepositorio transacaoRepositorio
        ) : IRequestHandler<ObterFaturaRequest, ApplicationResponse<FaturaDTO>>
    {
        private readonly IValidator<ObterFaturaRequest> _validator = validator;
        private readonly IDataFechamentoCartaoRepositorio _dataFechamentoCartaoRepositorio = dataFechamentoCartaoRepositorio;
        private readonly ICartaoCreditoRepositorio _cartaoCreditoRepositorio = cartaoCreditoRepositorio;
        private readonly ITransacaoRepositorio _transacaoRepositorio = transacaoRepositorio;
        public async Task<ApplicationResponse<FaturaDTO>> Handle(ObterFaturaRequest request, CancellationToken cancellationToken)
        {
            var response = new ApplicationResponse<FaturaDTO>();
            try
            {
                var validacao = await _validator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                var cartao = await _cartaoCreditoRepositorio.GetById(request.CartaoId);

                if (cartao is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Não foi possivel encontrar o cartão");
                }

                var alteracoesFechamento = _dataFechamentoCartaoRepositorio
                .GetByExpression(c => c.CartaoCreditoId == request.CartaoId)
                .OrderByDescending(c => c.DataAlteracao);

                var dataFechamentoVigente = ObterDataFechamentoVigente(
                    alteracoesFechamento,
                    request.DataReferencia,
                    cartao.DataFechamento);
                var (dataInicio, dataFim) = ObterPeriodoFatura(dataFechamentoVigente, request.DataReferencia);

                var transacoes = _transacaoRepositorio
                    .GetByExpression(t => t.Data >= dataInicio && t.Data < dataFim)
                    .ToList();

                var totalValue = transacoes.Sum(t => t.Valor);

                var faturaDTO = new FaturaDTO
                {
                    Valor = totalValue,
                    Transacoes = transacoes.Select(t => t.ToMapper()).ToList(),
                    Fechamento = dataFim,
                    Vencimento = dataFim.AddDays((cartao.DataVencimento - cartao.DataFechamento).Days),
                    CartaoCredito = cartao.ToMapper()
                };

                response.AddData(faturaDTO);
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao obter fatura");
            }
            return response;
        }

        private static DateTime ObterDataFechamentoVigente(
            IEnumerable<DataFechamentoCartaoCredito> alteracoes,
            DateTime dataReferencia,
            DateTime dataFechamentoAtual)
        {
            return alteracoes.FirstOrDefault(c => c.DataAlteracao <= dataReferencia)?.DataFechamentoAnterior
                ?? dataFechamentoAtual;
        }

        private static (DateTime dataInicio, DateTime dataFim) ObterPeriodoFatura(DateTime dataFechamento, DateTime dataReferencia)
        {
            DateTime dataInicio;
            if (dataReferencia.Day < dataFechamento.Day)
            {
                dataInicio = new DateTime(dataReferencia.Year, dataReferencia.Month, dataFechamento.Day).AddMonths(-1);
            }
            else
            {
                dataInicio = new DateTime(dataReferencia.Year, dataReferencia.Month, dataFechamento.Day);
            }
            var dataFim = dataInicio.AddMonths(1);
            return (dataInicio, dataFim);
        }

    }
}
