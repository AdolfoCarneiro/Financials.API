using FluentValidation;

namespace Financials.Services.RequestsResponses.Conta.Validators
{
    public class AtualizarContaRequestValidator : AbstractValidator<CriarContaRequest>
    {
        public AtualizarContaRequestValidator()
        {
            RuleFor(x => x.Nome).MinimumLength(2).WithMessage("Nome inválido");
        }
    }
}
