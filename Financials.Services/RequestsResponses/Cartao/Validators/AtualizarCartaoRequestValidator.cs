using FluentValidation;

namespace Financials.Services.RequestsResponses.Cartao.Validators
{
    public class AtualizarCartaoRequestValidator : AbstractValidator<AtualizarCartaoRequest>
    {
        public AtualizarCartaoRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id inválido").NotNull().WithMessage("Id inválido");
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MinimumLength(2).WithMessage("Nome precisa de pelo menos 2 carateres");

            RuleFor(x => x.DataFechamento).NotEmpty().WithMessage("Data de fechamento é obrigatória");
        }
    }
}
