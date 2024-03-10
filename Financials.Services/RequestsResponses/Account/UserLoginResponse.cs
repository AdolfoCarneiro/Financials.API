using Financials.Core.VO;

namespace Financials.Services.RequestsResponses.Account
{
    public class UserLoginResponse
    {
        public TokenVO Token{ get; set; } = new TokenVO();
        public UsuarioVO Usuario { get; set; } = new UsuarioVO();
    }
}