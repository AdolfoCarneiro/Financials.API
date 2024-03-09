using FluentValidation;

namespace Financials.Services.RequestsResponses.Account.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty()
                .WithMessage("O campo Usuário é obrigatório");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("O campo Senha é obrigatório");
        }
    }
}
