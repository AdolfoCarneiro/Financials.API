using Financials.Core.Entity;
using Financials.Infrastructure.Repositorio.Interfaces;
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
    public class PingController(ITransacaoRepositorio transacaoRepositorio,
        IContaRespositorio contaRepositorio,
        ICategoriaRepositorio categoriaRepositorio) : ControllerBase
    {
        private readonly IContaRespositorio _contaRepositorio = contaRepositorio;
        private readonly ITransacaoRepositorio _transacaoRepositorio = transacaoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio = categoriaRepositorio;

        /// <summary>
        /// Endpoint de teste da api
        /// </summary>
        [HttpGet]
        public async Task<string> Ping()
        {
            var conta = new Conta()
            {
                Nome = "das",
            };

            conta = await _contaRepositorio.Insert(conta);

            var categoria = new Categoria()
            {
                Nome = "!teste"
            };

            categoria = await _categoriaRepositorio.Insert(categoria);

            var transacao = new Transacao()
            {
                ContaId = conta.Id,
                Data = DateTime.Now,
                Valor = 10,
                CategoriaId = categoria.Id,
            };

            transacao = await _transacaoRepositorio.Insert(transacao);

            return conta.Id.ToString();

        }
        [HttpGet("{id}")]
        public async Task<string> Ping(Guid id)
        {
            var conta = await _contaRepositorio.GetById(id);

            var transacao = conta.Transacoes.FirstOrDefault();

            return conta.Id.ToString();

        }

        [HttpPost("{id}")]
        public async Task<string> Pings(Guid id)
        {
            var conta = await _contaRepositorio.GetById(id,new string[] {"Transacoes"});

            var transacao = conta.Transacoes.FirstOrDefault();

            return conta.Id.ToString();

        }
    }
}
