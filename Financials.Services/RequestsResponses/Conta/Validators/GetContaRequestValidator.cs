using FluentValidation;

namespace Financials.Services.RequestsResponses.Conta.Validators
{
    public class GetContaRequestValidator : AbstractValidator<GetContaRequest>
    {
        public GetContaRequestValidator()
        {
            RuleFor(x => x.ContaId)
                .NotNull()
                .NotEmpty();
        }
    }
}
