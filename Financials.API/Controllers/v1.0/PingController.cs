using Financials.Services.RequestsResponses.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.API.Controllers.v1._0
{
    /// <summary>
    /// Ping Controller
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]/[Action]")]
    public class PingController : ControllerBase
    {
        /// <summary>
        /// Retorna 400
        /// </summary>
        [HttpGet]
        public ActionResult<ApplicationResponse<string>> Ping400()
        {
            var response = new ApplicationResponse<string>();
            response.AddError(ResponseErrorType.ValidationError,"Requisição inválida");
            return this.GetResponse(response);
        }

        /// <summary>
        /// Retorna 500
        /// </summary>
        [HttpGet]
        public ActionResult<ApplicationResponse<string>> Ping500()
        {
            var response = new ApplicationResponse<string>();
            response.AddError(ResponseErrorType.InternalError, "Erro ao efetuar ping");
            return this.GetResponse(response);
        }

        /// <summary>
        /// Retorna 200
        /// </summary>
        [HttpGet]
        public ActionResult<ApplicationResponse<string>> Ping200()
        {
            var response = new ApplicationResponse<string>();
            response.AddData("pong");
            return this.GetResponse(response);
        }

        /// <summary>
        /// Retorna 404
        /// </summary>
        [HttpGet]
        public ActionResult<ApplicationResponse<string>> Ping404()
        {
            var response = new ApplicationResponse<string>();
            response.AddError(ResponseErrorType.NotFound, "Servidor não encontrado");
            return this.GetResponse(response);
        }

        /// <summary>
        /// Retorna 403
        /// </summary>
        [HttpGet]
        public ActionResult<ApplicationResponse<string>> Ping403()
        {
            var response = new ApplicationResponse<string>();
            response.AddError(ResponseErrorType.Forbidden, "Usuário não autorizado");
            return this.GetResponse(response);
        }

        /// <summary>
        /// Retorna 401
        /// </summary>
        [HttpGet]
        public ActionResult<ApplicationResponse<string>> Ping401()
        {
            var response = new ApplicationResponse<string>();
            response.AddError((ResponseErrorType)401, "Usuário não autenticado");
            return Unauthorized(response);
        }
    }
}
