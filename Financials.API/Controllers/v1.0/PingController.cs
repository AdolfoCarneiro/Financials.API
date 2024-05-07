using Financials.Core.Entity;
using Financials.Core.VO;
using Financials.Infrastructure.Repositorio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpPost]
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

        [HttpGet]
        public async Task<IEnumerable<TransacaoVO>> GetTransacoes()
        {
            var filtro = "";

            // Obtém todas as transações como uma lista, sem filtragem ou otimização.
            var transacoes = _transacaoRepositorio.GetAll();

            // Obtém um IQueryable para transações, permitindo construir consultas otimizadas que serão executadas no banco de dados.
            var transacoesQueryable = _transacaoRepositorio.GetAllQueryable();

            // INCORRETO: Primeiro converte o IQueryable para uma lista, trazendo todos os dados para a memória, e depois aplica a projeção.
            // Isso resulta em uma carga desnecessária de dados na memória antes da transformação.
            var voListErrado = transacoesQueryable.ToList().Select(t => new TransacaoVO()
            {
                Descricao = t.Descricao,
                Id = t.Id,
                Valor = t.Valor,
            });

            // INCORRETO: Aplica a projeção diretamente na lista de transações, sem a possibilidade de otimização pelo banco de dados.
            // Embora funcione, não é a abordagem mais eficiente para grandes volumes de dados.
            var voListErrado2 = transacoes.Select(t => new TransacaoVO()
            {
                Descricao = t.Descricao,
                Id = t.Id,
                Valor = t.Valor,
            });

            // CORRETO: Aplica a projeção diretamente no IQueryable, permitindo que a transformação seja parte da consulta ao banco de dados.
            // Isso é mais eficiente, pois apenas os dados necessários são solicitados e transformados.
            var voListCerto = transacoesQueryable.Select(t => new TransacaoVO()
            {
                Descricao = t.Descricao,
                Id = t.Id,
                Valor = t.Valor,
            });

            // Materializa a consulta ao banco de dados, executando a operação e trazendo os dados para a memória.
            var voListMemoria = voListCerto.ToList();

            // INCORRETO: A ordem das operações não é ideal. Aplicar Skip e Take antes de OrderBy pode resultar em uma seleção de dados inconsistente.
            var transacoesPaginadasErradas = transacoesQueryable.Where(t => t.Descricao.Contains(filtro))
                .Skip(100)
                .Take(50)
                .OrderByDescending(t => t.Data);

            // CORRETO: Primeiro ordena os dados e depois aplica a paginação com Skip e Take, garantindo a correta ordenação e seleção dos dados.
            var transacoesPaginadasCertas = transacoesQueryable.Where(t => t.Descricao.Contains(filtro))
                .OrderByDescending(t => t.Data)
                .Skip(100)
                .Take(50);

            // INCORRETO: A tentativa de agrupar por CategoriaId e depois incluir detalhes da categoria pode não funcionar como esperado,
            // especialmente se a consulta for complexa ou se o EF Core não conseguir otimizar a inclusão após o agrupamento.
            var transacoesPorCategoriaErrado = transacoesQueryable.Where(t => t.Descricao.Contains(filtro))
                .Include(t => t.Categoria)
                .GroupBy(t => t.CategoriaId)
                .Select(g => new TransacaoCategoriaVO
                {
                    Id = g.First().Categoria.Id,
                    Descricao = g.First().Categoria.Nome,
                    Transacoes = g.Select(t => new TransacaoVO
                    {
                        Id = t.Id,
                        Descricao = t.Descricao,
                        Valor = t.Valor
                    })
                }).ToList();

            // CORRETO: Inicia a consulta pelas categorias, incluindo as transações relacionadas, e depois projeta o resultado.
            // Esta abordagem é mais direta e eficiente, especialmente para modelar relações um-para-muitos no EF Core.
            var transacoesPorCategoriaCerto = _categoriaRepositorio.GetAllQueryable()
                .Include(t => t.Transacoes)
                .Select(c => new TransacaoCategoriaVO
                {
                    Id = c.Id,
                    Descricao = c.Nome,
                    Transacoes = c.Transacoes.Select(t => new TransacaoVO
                    {
                        Id = t.Id,
                        Descricao = t.Descricao,
                        Valor = t.Valor
                    })
                }).ToList();


            // Essas abordagens são menos eficientes porque:
            // 1. OrderBy/OrderByDescending irá ordenar todos os registros no banco de dados, o que pode ser custoso em termos de performance.
            // 2. FirstOrDefault irá então buscar o primeiro registro da lista ordenada, o que implica em carregar todos os registros ordenados em memória antes de selecionar o primeiro.
            // Em contraste, MaxBy e MinBy são otimizados para executar essas operações diretamente no banco de dados, sem necessidade de ordenação completa ou carregamento em memória.

            // Consulta Ineficiente
            var consultaIneficiente = transacoesQueryable
                .OrderBy(t => t.Data) // Ordena todos os registros, o que é menos eficiente.
                .Where(t => t.Descricao.Contains(filtro)) // Filtro aplicado após ordenação.
                .ToList(); // Executa a consulta e carrega os resultados.

            // Consulta Eficiente
            var consultaEficiente = transacoesQueryable
                .Where(t => t.Descricao.Contains(filtro)) // Filtro primeiro reduz o conjunto de dados.
                .OrderBy(t => t.Data) // Ordena os registros já filtrados.
                .ToList(); // Executa a consulta otimizada.

            // Resumo: Aplicar 'Where' antes de 'OrderBy' reduz o volume de dados processados,
            // melhorando a performance da consulta.


            // Retorna a lista de TransacaoVO corretamente projetada e otimizada para a memória.
            return voListMemoria;

        }
    }
}
