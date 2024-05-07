using FluentValidation;

namespace Financials.Services.RequestsResponses.Conta.Validators
{
    public class ContaRequestValidator : AbstractValidator<ContaRequest>
    {
        public ContaRequestValidator()
        {
            RuleFor(x => x.Nome).MinimumLength(2).WithMessage("Nome inválido");
        }
    }
}
