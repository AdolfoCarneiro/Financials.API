using Financials.Core.DTO;
using Financials.Core.Entity;
using System.Diagnostics.CodeAnalysis;

namespace Financials.Services.Mappers
{
    [ExcludeFromCodeCoverage]
    public static class ContaMapper
    {
        public static ContaDTO ToMapper(this Conta conta)
        {
            return new ContaDTO
            {
                Id = conta.Id,
                Nome = conta.Nome,
                SaldoInicial = conta.SaldoInicial,
                Tipo = conta.Tipo,
                Saldo = conta.Saldo,
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
