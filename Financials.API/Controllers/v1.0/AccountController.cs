using Financials.Services.Interfaces.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financials.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ICriarUsuario _criarUsuario;

        public AccountController(
            ICriarUsuario criarUsuario)
        {
            _criarUsuario = criarUsuario;
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationResponse<UsuarioResponse>>> CriarUsuario([FromBody] UsuarioRequest request)
        {
            var response = await _criarUsuario.Run(request);
            return this.GetResponse(response);
        }
    }
}
