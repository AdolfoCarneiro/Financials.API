using Financials.Core.DTO;
using Financials.Core.Entity;

namespace Financials.Services.Mappers
{

    public static class TransacaoMapper
    {
        public static TransacaoDTO ToMapper(this Transacao transacao)
        {
            return new TransacaoDTO
            {
                Id = transacao.Id,
                Valor = transacao.Valor,
                Descricao = transacao.Descricao,
                Data = transacao.Data,
                Tipo = transacao.Tipo,
                ContaId = transacao.ContaId,
                CategoriaId = transacao.CategoriaId,
                CartaoCreditoId = transacao.CartaoCreditoId,
                Recorrente = transacao.Recorrente,
                FrequenciaRecorrencia = transacao.FrequenciaRecorrencia,
                TotalParcelas = transacao.TotalParcelas,
                NumeroParcela = transacao.NumeroParcela,
                UserId = transacao.UserId
            };
        }
    }
}
