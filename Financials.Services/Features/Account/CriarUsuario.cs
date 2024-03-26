using Financials.Core.Entity;
using Financials.Services.Interfaces.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Financials.Services.Features.Account
{
    public class CriarUsuario : ICriarUsuario
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IValidator<UsuarioRequest> _validator;

        public CriarUsuario(
            UserManager<ApplicationUser> userManager,
            IValidator<UsuarioRequest> validator,
            RoleManager<IdentityRole> roleManager
        )
        {
            _userManager = userManager;
            _validator = validator;
            _roleManager = roleManager;
        }

        public async Task<ApplicationResponse<UsuarioResponse>> Run(UsuarioRequest request)
        {
            var response = new ApplicationResponse<UsuarioResponse>();
            try
            {
                var validate = await _validator.ValidateAsync(request);
                if (!validate.IsValid)
                {
                    response.AddError(validate.Errors);
                    return response;
                }

                var applicationUser = new ApplicationUser()
                {
                    Email = request.Email,
                    Nome = request.Nome,
                    DataNascimento = request.DataNascimento,
                    PhoneNumber = request.Telefone,
                    UserName = request.Email,
                };

                var userResult = await _userManager.CreateAsync(applicationUser,request.Senha);

                foreach(var role in request.Roles)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);

                    if (!roleExists)
                    {
                        var identityRole = new IdentityRole(role);
                        var roleResult = await _roleManager.CreateAsync(identityRole);
                    }

                    await _userManager.AddToRoleAsync(applicationUser, role);
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex, "Ocorreu um erro ao criar a usuário");
            }
            return response;
        }

    }
}
