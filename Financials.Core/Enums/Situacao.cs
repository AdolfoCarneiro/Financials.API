using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Enums
{
    public enum Situacao
    {
        [Display(Name = "Ativo")]
        Ativo = 0,

        [Display(Name = "Inativo")]
        Inativo = 1,
    }
}
