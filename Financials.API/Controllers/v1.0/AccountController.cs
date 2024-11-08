﻿using Financials.Services.Features.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
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
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UsuarioResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<UsuarioResponse>>> CriarUsuario([FromBody] UsuarioRequest request)
        {
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserLoginResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(UserLoginResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(UserLoginResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<UserLoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }

        //[HttpPost("esqueciSenha")]
        //[AllowAnonymous]
        //[ProducesResponseType(typeof(ApplicationResponse<object>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ApplicationResponse<object>), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(ApplicationResponse<object>), (int)HttpStatusCode.InternalServerError)]
        //public async Task<ActionResult<ApplicationResponse<object>>> EmailRedefinirSenha([FromBody] string email)
        //{
        //    var response = await _redefinirSenha.Run(email);
        //    return this.GetResponse(response);
        //}

        [HttpPost("resetSenha")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApplicationResponse<object>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApplicationResponse<object>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApplicationResponse<object>), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ApplicationResponse<object>>> RedefinirSenha([FromBody] RedefinirSenhaRequest request)
        {
            var response = await _mediator.Send(request);
            return this.GetResponse(response);
        }
    }
}
