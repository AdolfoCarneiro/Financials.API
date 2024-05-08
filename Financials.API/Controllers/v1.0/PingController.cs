using Financials.Core.Entity;
using Financials.Core.VO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Financials.API.Controllers.v1._0
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PingController : ControllerBase
    {

        /// <summary>
        /// Endpoint de teste da api
        /// </summary>
        [HttpPost]
        public async Task<string> Ping()
        {
            return "pong";
        }
    }
}
