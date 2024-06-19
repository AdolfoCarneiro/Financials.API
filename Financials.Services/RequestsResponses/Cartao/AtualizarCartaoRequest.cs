using System.Text.Json.Serialization;

namespace Financials.Services.RequestsResponses.Cartao
{
    public class AtualizarCartaoRequest : RegistrarCartaoRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
