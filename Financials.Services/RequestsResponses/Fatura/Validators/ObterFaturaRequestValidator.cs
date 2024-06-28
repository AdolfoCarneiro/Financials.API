using FluentValidation;

namespace Financials.Services.RequestsResponses.Fatura.Validators
{
    public class ObterFaturaRequestValidator : AbstractValidator<ObterFaturaRequest>
    {
        public ObterFaturaRequestValidator()
        {
            RuleFor(r => r.CartaoId).NotEmpty();
        }
    }
}
