using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Enums
{
    public enum TipoConta
    {
        [Display(Name = "Corrente")]
        Corrente = 0,

        [Display(Name = "Poupança")]
        Poupanca = 1,

        [Display(Name = "Carteira")]
        Carteira = 2,

        [Display(Name = "Investimento")]
        Investimento = 3,

        [Display(Name = "Cofrinho")]
        Cofrinho = 4,

        [Display(Name = "Outros")]
        Outros = 5,

    }
}
