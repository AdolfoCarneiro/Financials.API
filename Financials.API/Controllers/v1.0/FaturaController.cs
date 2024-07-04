using Financials.Core.DTO;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Fatura;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financials.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class FaturaController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<ApplicationResponse<FaturaDTO>>> GetFatura([FromQuery] DateTime dataReferencia, [FromQuery] Guid cartaoId)
        {
            ObterFaturaRequest request = new ObterFaturaRequest() { CartaoId = cartaoId, DataReferencia = dataReferencia };
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }
    }
}
