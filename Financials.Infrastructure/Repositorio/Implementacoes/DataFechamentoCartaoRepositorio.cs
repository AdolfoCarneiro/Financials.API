using Financials.Core.Entity;
using Financials.Infrastructure.Context;
using Financials.Infrastructure.Repositorio.Interfaces;

namespace Financials.Infrastructure.Repositorio.Implementacoes
{
    public class DataFechamentoCartaoRepositorio(FinancialsDbContext dbContext) : RepositorioGenerico<DataFechamentoCartaoCredito>(dbContext), IDataFechamentoCartaoRepositorio
    {
    }
}
