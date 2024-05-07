using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Core.VO
{
    public class TransacaoVO
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
    }

    public class TransacaoCategoriaVO
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public IEnumerable<TransacaoVO> Transacoes { get; set; }
    }
}
