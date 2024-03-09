namespace Financials.Services.RequestsResponses.Account
{
    public class UsuarioRequest
    {

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public List<string> Roles { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
