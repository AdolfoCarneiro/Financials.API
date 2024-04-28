using Financials.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class Categoria
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Icone { get; set; }
        public string Cor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; }
    }
}
