using FluentValidation;

namespace Financials.Services.RequestsResponses.Conta.Validators
{
    public class CriarContaRequestValidator : AbstractValidator<CriarContaRequest>
    {
        public CriarContaRequestValidator()
        {
            RuleFor(x => x.Nome).MinimumLength(2).WithMessage("Nome inválido");
        }
    }
}
