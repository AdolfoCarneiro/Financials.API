using Financials.Core.Entity;
using Financials.Core.VO;
using Financials.Infrastructure.Configuraton;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Financials.Services.Features.Account
{
    public class GerarTokens(
        UserManager<ApplicationUser> userManager,
        IOptions<JWTConfiguration> configuration)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly JWTConfiguration _configuration = configuration.Value;

        public virtual async Task<TokenVO> Run(ApplicationUser user)
        {
            var claims = new List<Claim>(){
                new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            };
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var acessTokenExpiration = DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpirationMinutes);
            var refreshTokenExpiration = DateTime.UtcNow.AddMinutes(_configuration.RefreshTokenExpirationMinutes);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: _configuration.Issuer,
               audience: _configuration.Audience,
               claims: claims,
               expires: acessTokenExpiration,
               signingCredentials: creds);

            JwtSecurityToken refreshToken = new JwtSecurityToken(
               issuer: _configuration.Issuer,
               audience: _configuration.Audience,
               claims: claims,
               expires: refreshTokenExpiration,
               signingCredentials: creds);

            return new TokenVO()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                Expiration = acessTokenExpiration
            };
        }
    }
}
