using Financials.Core.DTO;
using Financials.Infrastructure.Helper;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using MediatR;
using System.Linq.Expressions;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Features.Conta
{
    public class GetContasByFilter(IContaRespositorio contaRepositorio) : IRequestHandler<GetContasByFilterRequest, ApplicationResponse<IEnumerable<ContaDTO>>>
    {
        private readonly IContaRespositorio _contaRepositorio = contaRepositorio;
        public async Task<ApplicationResponse<IEnumerable<ContaDTO>>> Handle(GetContasByFilterRequest request, CancellationToken cancellationToken)
        {
            var response = new ApplicationResponse<IEnumerable<ContaDTO>>();
            try
            {
                Expression<Func<Entity.Conta, bool>> filtro = conta => true;

                if (!string.IsNullOrEmpty(request.Filtro))
                {
                    Expression<Func<Entity.Conta, bool>> filtroNome = conta => conta.Nome.Equals(request.Filtro, StringComparison.CurrentCultureIgnoreCase);
                    filtro = filtro == null ? filtroNome : ExpressionHelper.CombinaFiltrosAnd(filtro, filtroNome);
                }

                if (request.TipoConta.HasValue)
                {
                    Expression<Func<Entity.Conta, bool>> filtroTipo = conta => conta.Tipo == request.TipoConta;
                    filtro = filtro == null ? filtroTipo : ExpressionHelper.CombinaFiltrosAnd(filtro, filtroTipo);
                }

                var contas = _contaRepositorio.GetByExpression(filtro).ToList() ?? Enumerable.Empty<Entity.Conta>();

                var contasDto = contas.Select(c => c.ToMapper()).ToList();

                response.AddData(contasDto);
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao obter as contas");
            }
            return response;

        }
    }
}
