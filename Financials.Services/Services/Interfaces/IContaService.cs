using Financials.Core.DTO;
using Financials.Core.Enums;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;

namespace Financials.Services.Services.Interfaces
{
    public interface IContaService
    {
        Task<ApplicationResponse<ContaDTO>> CriarContaAsync(CriarContaRequest request, CancellationToken cancellationToken = default);
        Task<ApplicationResponse<ContaDTO>> AtualizarContaAsync(Guid id, AtualizarContaRequest request, CancellationToken cancellationToken = default);
        Task<ApplicationResponse<ContaDTO>> ObterContaAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ApplicationResponse<List<ContaDTO>>> ObterContasAsync(string? nome = null, TipoConta? tipo = null, CancellationToken cancellationToken = default);
    }
}
