namespace Financials.Services.RequestsResponses.Account
{
    public class RedefinirSenhaRequest
    {
        public string NovaSenha { get; set; }
        public Guid UsuarioId { get; set; }
        public string Token { get; set; }
    }
}
