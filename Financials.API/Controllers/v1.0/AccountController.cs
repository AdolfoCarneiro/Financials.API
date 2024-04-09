using Financials.Services.Interfaces;
using Financials.Services.Interfaces.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Financials.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly ICriarUsuario _criarUsuario;
        private readonly ILogin _login;

        public AccountController(
            ICriarUsuario criarUsuario,
            ILogin login)
        {
            _criarUsuario = criarUsuario;
            _login = login;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<UsuarioResponse>>> CriarUsuario([FromBody] UsuarioRequest request)
        {
            var response = await _criarUsuario.Run(request);
            return this.GetResponse(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserLoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(UserLoginResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UserLoginResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<UserLoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var response = await _login.Run(request);
            return this.GetResponse(response);
        }
    }
}
