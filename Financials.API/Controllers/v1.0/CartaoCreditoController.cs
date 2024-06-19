using Financials.Core.DTO;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Cartao;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Financials.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class CartaoCreditoController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;


        [HttpPost]
        [ProducesResponseType(typeof(ApplicationResponse<CartaoCreditoDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<CartaoCreditoDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<CartaoCreditoDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<CartaoCreditoDTO>>> CriarConta([FromBody] RegistrarCartaoRequest request)
        {
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }
    }
}
