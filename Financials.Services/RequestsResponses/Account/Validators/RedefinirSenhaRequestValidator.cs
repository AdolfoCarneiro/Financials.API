using FluentValidation;

namespace Financials.Services.RequestsResponses.Account.Validators
{
    public class RedefinirSenhaRequestValidator : AbstractValidator<RedefinirSenhaRequest>
    {
        public RedefinirSenhaRequestValidator()
        {
            RuleFor(x => x.UsuarioId)
          .NotEmpty()
          .WithMessage("O campo Id é obrigatório");

            RuleFor(x => x.NovaSenha)
                .NotEmpty()
                .WithMessage("O campo Senha é obrigatório")
                .MinimumLength(8).WithMessage("O campo Senha deve possuir pelo menos 8 caracteres")
                .MaximumLength(16).WithMessage("O campo Senha deve possuir no máximo 16 caracteres.")
                .Matches(@"[A-Z]+").WithMessage("O campo Senha deve possuir pelo menos uma letra maiúscula.")
                .Matches(@"[a-z]+").WithMessage("O campo Senha deve possuir pelo menos uma letra minúscula.")
                .Matches(@"[0-9]+").WithMessage("O campo Senha deve possuir pelo menos um número.")
                .Matches(@"[&\(\)\¨\%\#\@\$\!\?\*\.]+").WithMessage("O campo Senha deve possuir pelo menos um caractere especial.");
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("O campo Token é obrigatório");
        }
    }
}
