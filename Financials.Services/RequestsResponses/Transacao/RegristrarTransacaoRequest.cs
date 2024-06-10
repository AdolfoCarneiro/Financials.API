using Financials.Core.DTO;
using Financials.Core.Enums;
using Financials.Services.RequestsResponses.Base;
using MediatR;

namespace Financials.Services.RequestsResponses.Transacao
{
    public class RegristrarTransacaoRequest : IRequest<ApplicationResponse<TransacaoDTO>>
    {
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public Guid? ContaId { get; set; }
        public Guid CategoriaId { get; set; }
        public Guid? CartaoCreditoId { get; set; }
        public Guid? FaturaId { get; set; }
        public bool Recorrente { get; set; } = false;
        public FrequenciaRecorrencia FrequenciaRecorrencia { get; set; }
        public int TotalParcelas { get; set; } = 1;
    }
}
