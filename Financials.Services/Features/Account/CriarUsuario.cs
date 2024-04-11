using Financials.Core.Entity;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Financials.Services.Features.Account
{
    public class CriarUsuario(
        UserManager<ApplicationUser> userManager,
        IValidator<UsuarioRequest> validator,
        RoleManager<IdentityRole> roleManager
        )
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IValidator<UsuarioRequest> _validator = validator;

        public async Task<ApplicationResponse<UsuarioResponse>> Run(UsuarioRequest request)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            var response = new ApplicationResponse<UsuarioResponse>();
            try
            {
                var validate = await _validator.ValidateAsync(request);
                if (!validate.IsValid)
                {
                    response.AddError(validate.Errors);
                    return response;
                }

                applicationUser = new ApplicationUser()
                {
                    Email = request.Email,
                    Nome = request.Nome,
                    DataNascimento = request.DataNascimento,
                    PhoneNumber = request.Telefone,
                    UserName = request.Email,
                };

                var userResult = await _userManager.CreateAsync(applicationUser,request.Senha);
                if (!userResult.Succeeded)
                {
                    response.AddError(userResult.Errors.ToList());
                    return response;
                }

                foreach(var role in request.Roles)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(role);

                    if (!roleExists)
                    {
                        response.AddError(ResponseErrorType.NotFound, $"A role informada({role}) não foi encontrada");
                        return response;
                    }

                    var addUserToRoleResult = await _userManager.AddToRoleAsync(applicationUser, role);
                    if (!addUserToRoleResult.Succeeded)
                    {
                        response.AddError(addUserToRoleResult.Errors.ToList());
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                await _userManager.DeleteAsync(applicationUser);
                response.AddError(ex, "Ocorreu um erro ao criar a usuário");
            }
            return response;
        }

    }
}
