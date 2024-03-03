using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.API.Controllers.v1._0
{
    /// <summary>
    /// Ping Controller
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PingController : ControllerBase
    {
        /// <summary>
        /// Endpoint de teste da api
        /// </summary>
        [HttpGet]
        public string Ping()
        {
            return "pong";
        }
    }
}
