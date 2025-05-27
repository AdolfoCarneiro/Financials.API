using Financials.Core.DTO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Transacao;
using FluentValidation;
using MediatR;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Features.Transacao;

public class RegistrarTransacao(
        IValidator<RegristrarTransacaoRequest> validator,
        ITransacaoRepositorio transacaoRepositorio,
        IUnitOfWork unitOfWork
    )
    : IRequestHandler<RegristrarTransacaoRequest, ApplicationResponse<TransacaoDTO>>
{
    private readonly IValidator<RegristrarTransacaoRequest> _validator = validator;
    private readonly ITransacaoRepositorio _transacaoRepositorio = transacaoRepositorio;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ApplicationResponse<TransacaoDTO>> Handle(RegristrarTransacaoRequest request, CancellationToken cancellationToken)
    {
        var response = new ApplicationResponse<TransacaoDTO>();
        try
        {
            var validacao = await _validator.ValidateAsync(request, cancellationToken);
            if (!validacao.IsValid)
            {
                response.AddError(validacao.Errors);
                return response;
            }
            var grupoTransacao = Guid.NewGuid();

            if (request.Recorrente)
            {
                var transacao = new Entity.Transacao()
                {
                    Data = request.Data,
                    CategoriaId = request.CategoriaId,
                    Descricao = request.Descricao,
                    CartaoCreditoId = request.CartaoCreditoId,
                    ContaId = request.ContaId,
                    FrequenciaRecorrencia = request.FrequenciaRecorrencia,
                    TotalParcelas = request.TotalParcelas,
                    Recorrente = request.Recorrente,
                    GrupoTransacao = grupoTransacao,
                    Tipo = request.Tipo,
                    Valor = request.Valor,
                };

                transacao = await _transacaoRepositorio.Insert(transacao);

                var result = transacao.ToMapper();
                response.AddData(result);
            }
            else
            {
                var transacoes = new List<Entity.Transacao>();

                decimal valorBaseParcela = Math.Floor(request.Valor / request.TotalParcelas * 100) / 100; // Arredondamento para centavos
                decimal somaParcelas = valorBaseParcela * request.TotalParcelas;
                decimal diferenca = request.Valor - somaParcelas; // Diferenca para ajustar a última parcela

                DateTime dataParcela = request.Data;

                for (var i = 1; i <= request.TotalParcelas; i++)
                {
                    decimal valorParcela = valorBaseParcela;
                    if (i == request.TotalParcelas) // Ajusta a última parcela
                    {
                        valorParcela += diferenca;
                    }

                    string descricao = request.TotalParcelas > 1 ? $"{request.Descricao} {i}/{request.TotalParcelas}" : request.Descricao;
                    var transacao = new Entity.Transacao()
                    {
                        Data = dataParcela,
                        CategoriaId = request.CategoriaId,
                        Descricao = descricao,
                        CartaoCreditoId = request.CartaoCreditoId,
                        ContaId = request.ContaId,
                        FrequenciaRecorrencia = request.FrequenciaRecorrencia,
                        NumeroParcela = i,
                        TotalParcelas = request.TotalParcelas,
                        Recorrente = request.Recorrente,
                        GrupoTransacao = grupoTransacao,
                        Tipo = request.Tipo,
                        Valor = valorParcela,
                    };

                    transacoes.Add(transacao);

                    dataParcela = Entity.Transacao.CalcularProximaData(dataParcela, request.FrequenciaRecorrencia);
                }

                await _transacaoRepositorio.Insert(transacoes);

                var result = transacoes.First().ToMapper();
                response.AddData(result);
            }
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            response.AddError(ex, "Erro ao registrar a transação");
        }
        return response;
    }

}