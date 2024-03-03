using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financials.API.Controllers.v2._0
{
    /// <summary>
    /// Ping Controller
    /// </summary>
    [ApiVersion("2.0")]
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
