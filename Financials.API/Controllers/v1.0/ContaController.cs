using Financials.Core.DTO;
using Financials.Core.Enums;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
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
    public class ContaController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<ContaDTO>>> CriarConta([FromBody] CriarContaRequest request)
        {
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<ContaDTO>>> AtualizarConta(Guid id, [FromBody] AtualizarContaRequest request)
        {
            request.Id = id;
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<ContaDTO>>> ObterConta(Guid id)
        {
            var request = new GetContaRequest()
            {
                ContaId = id,
            };
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<IEnumerable<ContaDTO>>>> ObterContas(
            [FromQuery] string filtro,
            [FromQuery] TipoConta? tipo
            )
        {
            var request = new GetContasByFilterRequest()
            {
                Filtro = filtro,
                TipoConta = tipo
            };
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }
    }
}
