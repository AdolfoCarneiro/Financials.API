using Financials.Core.DTO;
using Financials.Core.Entity;

namespace Financials.Services.Mappers
{
    public static class ContaMapper
    {
        public static ContaDTO ToMapper(this Conta conta)
        {
            return new ContaDTO
            {
                Id = conta.Id,
                Nome = conta.Nome,
                SaldoInicial = conta.SaldoInicial,
                Tipo = conta.Tipo
            };
        }

        public static Conta ToMapper(this ContaDTO conta)
        {
            return new Conta
            {
                Id = conta.Id,
                Nome = conta.Nome,
                SaldoInicial = conta.SaldoInicial,
                Tipo = conta.Tipo
            };
        }
    }
}
