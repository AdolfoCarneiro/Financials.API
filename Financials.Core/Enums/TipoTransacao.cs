using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Enums
{
    public enum TipoTransacao
    {
        [Display(Name = "Despesa")]
        Despesa = 0,

        [Display(Name = "Receita")]
        Receita = 1,
    }
}
