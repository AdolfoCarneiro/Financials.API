using System.Text.Json.Serialization;

namespace Financials.Services.RequestsResponses.Conta
{
    public class AtualizarContaRequest : CriarContaRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
