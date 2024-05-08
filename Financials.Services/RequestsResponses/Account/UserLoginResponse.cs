using Financials.Core.VO;
using System.Diagnostics.CodeAnalysis;

namespace Financials.Services.RequestsResponses.Account
{
    [ExcludeFromCodeCoverage]
    public class UserLoginResponse
    {
        public TokenVO Token{ get; set; } = new TokenVO();
        public UsuarioVO Usuario { get; set; } = new UsuarioVO();
    }
}