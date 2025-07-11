﻿using Financials.Core.Enums;
using Financials.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Financials.Core.Entity
{
    public class Conta : IUserOwnedResource
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoInicial { get; set; }
        public TipoConta Tipo { get; set; }
        public virtual ICollection<Transacao> Transacoes { get; set; } = [];
        public virtual ICollection<Transferencia> TransferenciasEnviadas { get; set; } = [];
        public virtual ICollection<Transferencia> TransferenciasRecebidas { get; set; } = [];
        public Guid UserId { get; set; }
        [NotMapped]
        public decimal Saldo
        {
            get
            {
                var receitas = Transacoes.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
                var despesas = Transacoes.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);
                var transferenciasEnviadas = TransferenciasEnviadas.Sum(t => t.Valor);
                var transferenciasRecebidas = TransferenciasRecebidas.Sum(t => t.Valor);

                return SaldoInicial + ((receitas + transferenciasRecebidas) - (despesas + transferenciasEnviadas));
            }
        }
    }
}
