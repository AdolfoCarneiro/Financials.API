using System.Diagnostics.CodeAnalysis;

namespace Financials.Core.VO
{
    [ExcludeFromCodeCoverage]
    public class TokenVO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
