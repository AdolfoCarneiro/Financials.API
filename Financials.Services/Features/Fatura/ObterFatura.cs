using Financials.Core.DTO;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Fatura;
using MediatR;

namespace Financials.Services.Features.Fatura
{
    public class ObterFatura : IRequestHandler<ObterFaturaRequest, ApplicationResponse<FaturaDTO>>
    {
        public async Task<ApplicationResponse<FaturaDTO>> Handle(ObterFaturaRequest request, CancellationToken cancellationToken)
        {
			var response = new ApplicationResponse<FaturaDTO>();
			try
			{

			}
			catch (Exception ex)
			{
				response.AddError(ex, "Erro ao obter fatura");
			}
			return response;
        }
    }
}
