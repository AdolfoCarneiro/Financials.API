using Financials.Core.Entity;
using Financials.Core.VO;
using Financials.Infrastructure.Configuraton;
using Financials.Services.Interfaces;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Financials.Services.Features.Account
{
    public class Login : ILogin
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IValidator<LoginRequest> _validator;
        private readonly JWTConfiguration _configuration;

        public Login(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IValidator<LoginRequest> validator,
            IOptions<JWTConfiguration> configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _validator = validator;
            _configuration = configuration.Value;
        }

        public async Task<ApplicationResponse<UserLoginResponse>> Run(LoginRequest request)
        {
            var response = new ApplicationResponse<UserLoginResponse>();
            try
            {
                var validacao = await _validator.ValidateAsync(request);
                if (!validacao.IsValid)
                {
                    response.AddError(validacao.Errors);
                    return response;
                }

                var user = await _userManager.FindByEmailAsync(request.Usuario);
                if (user is null)
                {
                    response.AddError(ResponseErrorType.InternalError, "Erro ao realizar login");
                }

                var loginResult = await _signInManager.PasswordSignInAsync(user, request.Senha, false, false);
                if (!loginResult.Succeeded)
                {
                    response.AddError(ResponseErrorType.InternalError, "Erro ao realizar login");
                }

                var token = await GenerateToken(user);

                UserLoginResponse responseData = new()
                {
                    Token = token
                };
                response.AddData(responseData);
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Erro ao realizar login");
            }
            return response;
        }

        private async Task<TokenVO> GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>(){
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
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
