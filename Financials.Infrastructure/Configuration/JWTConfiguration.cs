namespace Financials.Infrastructure.Configuraton
{
    public class JWTConfiguration
    {
        public string SecretKey{ get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
    }
}
