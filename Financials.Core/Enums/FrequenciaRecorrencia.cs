using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Enums
{
    public enum FrequenciaRecorrencia
    {
        [Display(Name = "Diária")]
        Diaria = 1,

        [Display(Name = "Semanal")]
        Semanal = 2,

        [Display(Name = "Quinzenal")]
        Quinzenal = 3,

        [Display(Name = "Mensal")]
        Mensal = 4,

        [Display(Name = "Bimestral")]
        Bimestral = 5,

        [Display(Name = "Trimestral")]
        Trimestral = 6,

        [Display(Name = "Semestral")]
        Semestral = 7,

        [Display(Name = "Anual")]
        Anual = 8
    }
}
