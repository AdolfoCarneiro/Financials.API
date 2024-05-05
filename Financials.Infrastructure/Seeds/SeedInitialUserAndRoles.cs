using Financials.Core.Entity;
using Financials.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Financials.Infrastructure.Seeds
{
    [ExcludeFromCodeCoverage]
    public class SeedInitialUserAndRoles(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager) : ISeedInitialUserAndRoles
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async void SeedUsers()
        {
            if (_userManager.FindByEmailAsync("admin@email.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin@email.com";
                user.Email = "admin@email.com";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.Nome = "Admin";

                IdentityResult result = _userManager.CreateAsync(user, "Pass123$").Result;

                if (result.Succeeded)
                {
                    Console.WriteLine("Usuário Admin criado com sucesso");
                    var roleResult = _userManager.AddToRoleAsync(user,RoleNames.Admin).Result;

                    if (roleResult.Succeeded)
                    {
                        Console.WriteLine("Usuário Admin adicionado a role Admin com sucesso");
                    }
                    else
                    {
                        Console.WriteLine("Erro ao adicionar usuário admin a role Admin");
                    }

                }
                else
                {
                    Console.WriteLine("um lobo");
                    Console.WriteLine("Não foi possível criar o usuário admin: " + result.Errors.FirstOrDefault());
                }
            }

            if (_userManager.FindByEmailAsync("default@email.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "default@email.com";
                user.Email = "default@email.com";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.Nome = "Default";

                IdentityResult result = _userManager.CreateAsync(user, "Pass123$").Result;

                if (result.Succeeded)
                {
                    Console.WriteLine("Usuário Default criado com sucesso");
                    var roleResult = _userManager.AddToRoleAsync(user, RoleNames.Default).Result;

                    if (roleResult.Succeeded)
                    {
                        Console.WriteLine("Usuário Default adicionado a role Default com sucesso");
                    }
                    else
                    {
                        Console.WriteLine("Erro ao adicionar usuário Default a role Default");
                    }
                }
                else
                {
                    Console.WriteLine("Não foi possível criar o usuário default: " + result.Errors.FirstOrDefault());
                }
            }

            if (_userManager.FindByEmailAsync("suporte@email.com").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "suporte@email.com";
                user.Email = "suporte@email.com";
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.Nome = "Suporte";

                IdentityResult result = _userManager.CreateAsync(user, "Pass123$").Result;

                if (result.Succeeded)
                {
                    Console.WriteLine("Usuário suporte criado com sucesso");
                    var roleResult = _userManager.AddToRoleAsync(user, RoleNames.Suporte).Result;

                    if (roleResult.Succeeded)
                    {
                        Console.WriteLine("Usuário Suporte adicionado a role Suporte com sucesso");
                    }
                    else
                    {
                        Console.WriteLine("Erro ao adicionar usuário Suporte a role Suporte");
                    }
                }
                else
                {
                    Console.WriteLine("Não foi possível criar o usuário suporte: " + result.Errors.FirstOrDefault());
                }
            }

        }

        public void SeedRoles()
        {
            if (_roleManager.FindByNameAsync(RoleNames.Admin).Result is null)
            {
                var role = new IdentityRole(RoleNames.Admin);
                IdentityResult result = _roleManager.CreateAsync(role).Result;

                if (result.Succeeded)
                {
                    Console.WriteLine("Role Admin criada com sucesso");
                }
                else
                {
                    Console.WriteLine("Não foi possível criar a role Admin: " + result.Errors.FirstOrDefault());
                }
            }
            if (_roleManager.FindByNameAsync(RoleNames.Default).Result is null)
            {
                var role = new IdentityRole(RoleNames.Default);
                IdentityResult result = _roleManager.CreateAsync(role).Result;

                if (result.Succeeded)
                {
                    Console.WriteLine("Role Default criada com sucesso");
                }
                else
                {
                    Console.WriteLine("Não foi possível criar a role Default: " + result.Errors.FirstOrDefault());
                }
            }
            if (_roleManager.FindByNameAsync(RoleNames.Suporte).Result is null)
            {
                var role = new IdentityRole(RoleNames.Suporte);
                IdentityResult result = _roleManager.CreateAsync(role).Result;

                if (result.Succeeded)
                {
                    Console.WriteLine("Role Suporte criada com sucesso");
                }
                else
                {
                    Console.WriteLine("Não foi possível criar a role Suporte: " + result.Errors.FirstOrDefault());
                }
            }

        }
    }
}
