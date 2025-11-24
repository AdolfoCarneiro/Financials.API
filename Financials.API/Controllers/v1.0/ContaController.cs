using Financials.Core.DTO;
using Financials.Core.Enums;
using Financials.Services.RequestsResponses.Base;
using Financials.Services.RequestsResponses.Conta;
using Financials.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Financials.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class ContaController : ControllerBase
    {
        private readonly IContaService _contaService;

        public ContaController(IContaService contaService)
        {
            _contaService = contaService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<ContaDTO>>> CriarConta([FromBody] CriarContaRequest request)
        {
            var response = await _contaService.CriarContaAsync(request);
            return this.GetResponse(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<ContaDTO>>> AtualizarConta([FromRoute] Guid id, [FromBody] AtualizarContaRequest request)
        {
            var response = await _contaService.AtualizarContaAsync(id, request);
            return this.GetResponse(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<ContaDTO>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<ContaDTO>>> ObterConta(Guid id)
        {
            var response = await _contaService.ObterContaAsync(id);
            return this.GetResponse(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApplicationResponse<List<ContaDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<List<ContaDTO>>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<List<ContaDTO>>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<List<ContaDTO>>>> ObterContas(
            [FromQuery] string? filtro,
            [FromQuery] TipoConta? tipo
            )
        {
            var response = await _contaService.ObterContasAsync(filtro, tipo);
            return this.GetResponse(response);
        }
    }
}
