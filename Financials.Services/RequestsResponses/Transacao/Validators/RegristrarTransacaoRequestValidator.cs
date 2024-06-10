using FluentValidation;

namespace Financials.Services.RequestsResponses.Transacao.Validators
{
    public class RegristrarTransacaoRequestValidator : AbstractValidator<RegristrarTransacaoRequest>
    {
        public RegristrarTransacaoRequestValidator()
        {
            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.")
                .NotEmpty().WithMessage("O valor é obrigatório.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.");

            RuleFor(x => x.CategoriaId)
                .NotEmpty().WithMessage("A categoria é obrigatória.");

            RuleFor(x => x.Data)
                .NotEmpty().WithMessage("A data é obrigatória.");

            RuleFor(x => x)
                .Must(x => x.ContaId.HasValue || x.CartaoCreditoId.HasValue)
                .WithMessage("É necessário informar ou a Conta ou o CartaoCredito.");
        }
    }
}
