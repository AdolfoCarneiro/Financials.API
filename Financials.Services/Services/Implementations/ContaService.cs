using Financials.Core.DTO;
using Financials.Core.Enums;
using Financials.Infrastructure.Helper;
using Financials.Infrastructure.Repositorio.Interfaces;
using Financials.Services.Mappers;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using Financials.Services.Services.Interfaces;
using FluentValidation;
using System.Linq.Expressions;
using Entity = Financials.Core.Entity;

namespace Financials.Services.Services.Implementations
{
    public class ContaService : IContaService
    {
        private readonly IContaRespositorio _contaRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CriarContaRequest> _criarContaValidator;
        private readonly IValidator<AtualizarContaRequest> _atualizarContaValidator;
        private readonly IValidator<GetContaRequest> _getContaValidator;

        public ContaService(
            IContaRespositorio contaRepositorio,
            IUnitOfWork unitOfWork,
            IValidator<CriarContaRequest> criarContaValidator,
            IValidator<AtualizarContaRequest> atualizarContaValidator,
            IValidator<GetContaRequest> getContaValidator)
        {
            _contaRepositorio = contaRepositorio;
            _unitOfWork = unitOfWork;
            _criarContaValidator = criarContaValidator;
            _atualizarContaValidator = atualizarContaValidator;
            _getContaValidator = getContaValidator;
        }

        public async Task<ApplicationResponse<ContaDTO>> CriarContaAsync(CriarContaRequest request, CancellationToken cancellationToken = default)
        {
            ApplicationResponse<ContaDTO> response = new();
            try
            {
                var validacao = await _criarContaValidator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                Entity.Conta conta = new()
                {
                    Nome = request.Nome,
                    SaldoInicial = request.SaldoInicial,
                    Tipo = request.Tipo,
                };

                conta = await _contaRepositorio.Insert(conta);

                response.AddData(conta.ToMapper());
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao criar a conta");
            }
            return response;
        }

        public async Task<ApplicationResponse<ContaDTO>> AtualizarContaAsync(Guid id, AtualizarContaRequest request, CancellationToken cancellationToken = default)
        {
            var response = new ApplicationResponse<ContaDTO>();
            try
            {
                request.Id = id; // Garantir que o ID do request é o mesmo da rota

                var validacao = await _atualizarContaValidator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                var conta = await _contaRepositorio.GetById(request.Id);

                if (conta is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Conta não encontrada");
                    return response;
                }

                conta.Tipo = request.Tipo;
                conta.Nome = request.Nome;
                conta.SaldoInicial = request.SaldoInicial;

                _contaRepositorio.Update(conta);
                await _unitOfWork.SaveChangesAsync();

                response.AddData(conta.ToMapper());
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao atualizar a conta");
            }
            return response;
        }

        public async Task<ApplicationResponse<ContaDTO>> ObterContaAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var response = new ApplicationResponse<ContaDTO>();
            try
            {
                var request = new GetContaRequest { ContaId = id };

                var validacao = await _getContaValidator.ValidateAsync(request, cancellationToken);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                var conta = await _contaRepositorio.GetById(id);

                if (conta is null)
                {
                    response.AddError(ResponseErrorType.NotFound, "Conta não encontrada");
                    return response;
                }

                response.AddData(conta.ToMapper());
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao obter a conta");
            }
            return response;
        }

        public async Task<ApplicationResponse<List<ContaDTO>>> ObterContasAsync(string? nome = null, TipoConta? tipo = null, CancellationToken cancellationToken = default)
        {
            var response = new ApplicationResponse<List<ContaDTO>>();
            try
            {
                Expression<Func<Entity.Conta, bool>> filtro = conta => true;

                if (!string.IsNullOrEmpty(nome))
                {
                    Expression<Func<Entity.Conta, bool>> filtroNome = conta => conta.Nome.Equals(nome, StringComparison.CurrentCultureIgnoreCase);
                    filtro = filtro == null ? filtroNome : ExpressionHelper.CombinaFiltrosAnd(filtro, filtroNome);
                }

                if (tipo.HasValue)
                {
                    Expression<Func<Entity.Conta, bool>> filtroTipo = conta => conta.Tipo == tipo;
                    filtro = filtro == null ? filtroTipo : ExpressionHelper.CombinaFiltrosAnd(filtro, filtroTipo);
                }

                var contas = _contaRepositorio.GetByExpression(filtro).ToList() ?? new List<Entity.Conta>();
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
