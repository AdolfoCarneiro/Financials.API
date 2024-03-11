using FluentValidation;

namespace Financials.Services.RequestsResponses.Account.Validators
{
    public class UsuarioRequestValidator : AbstractValidator<UsuarioRequest>
    {
        public UsuarioRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("O email informado é inválido");

            RuleFor(x => x.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .WithMessage("O nome informado é invãlido");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .MinimumLength(8).WithMessage("A senha tem menos de 8 caracteres");
        }
    }
}
