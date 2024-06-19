using FluentValidation;

namespace Financials.Services.RequestsResponses.Cartao.Validators
{
    public class RegistrarCartaoRequestValidator : AbstractValidator<RegistrarCartaoRequest>
    {
        public RegistrarCartaoRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(2).WithMessage("Nome precisa de pelo menos 2 carateres");

            RuleFor(x => x.DataFechamento).NotEmpty().WithMessage("Data de fechamento é obrigatória");
        }
    }
}
