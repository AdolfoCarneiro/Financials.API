using FluentValidation;

namespace Financials.Services.RequestsResponses.Conta.Validators
{
    public class AtualizarContaRequestValidator : AbstractValidator<AtualizarContaRequest>
    {
        public AtualizarContaRequestValidator()
        {
            RuleFor(x => x.Nome).MinimumLength(2).WithMessage("Nome inválido");
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id inválido");
        }
    }
}
