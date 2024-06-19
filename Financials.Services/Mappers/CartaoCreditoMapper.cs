using Financials.Core.DTO;
using Financials.Core.Entity;

namespace Financials.Services.Mappers
{
    public static class CartaoCreditoMapper
    {
        public static CartaoCreditoDTO ToMapper(this CartaoCredito cartaoCredito)
        {
            return new CartaoCreditoDTO
            {
                Limite = cartaoCredito.Limite,
                DataFechamento = cartaoCredito.DataFechamento,
                DataVencimento = cartaoCredito.DataVencimento,
                Id = cartaoCredito.Id,
                Nome = cartaoCredito.Nome,
            };
        }
    }
}
