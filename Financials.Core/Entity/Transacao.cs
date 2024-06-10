using Financials.Core.Enums;
using Financials.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Financials.Core.Entity
{
    [ExcludeFromCodeCoverage]
    public class Transacao : IUserOwnedResource
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public Guid? ContaId { get; set; }
        public virtual Conta Conta { get; set; }
        public Guid CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }
        public Guid? CartaoCreditoId { get; set; }
        public virtual CartaoCredito CartaoCredito { get; set; }
        public Guid? FaturaId { get; set; }
        public virtual Fatura Fatura { get; set; }
        public bool Recorrente { get; set; } = false;
        public FrequenciaRecorrencia FrequenciaRecorrencia { get; set; }
        public int TotalParcelas { get; set; } = 1;
        public int NumeroParcela { get; set; } = 1;
        //Todas as transações registradas juntas terão o mesmo valor nessa prop
        public Guid GrupoTransacao { get; set; }
        public Guid UserId { get; set; }

        public static DateTime CalcularProximaData(DateTime dataAtual, FrequenciaRecorrencia frequenciaRecorrencia)
        {
            return frequenciaRecorrencia switch
            {
                FrequenciaRecorrencia.Diaria => dataAtual.AddDays(1),
                FrequenciaRecorrencia.Semanal => dataAtual.AddDays(7),
                FrequenciaRecorrencia.Quinzenal => dataAtual.AddDays(15),
                FrequenciaRecorrencia.Mensal => dataAtual.AddMonths(1),
                FrequenciaRecorrencia.Bimestral => dataAtual.AddMonths(2),
                FrequenciaRecorrencia.Trimestral => dataAtual.AddMonths(3),
                FrequenciaRecorrencia.Anual => dataAtual.AddYears(1),
                _ => dataAtual
            };
        }
    }
}

