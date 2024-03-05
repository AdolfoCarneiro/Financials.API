namespace Financials.Core.VO
{
    public class TokenVO
    {
        public string AcessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
