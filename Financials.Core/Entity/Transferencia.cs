﻿using System.ComponentModel.DataAnnotations;

namespace Financials.Core.Entity
{
    public class Transferencia
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public Guid ContaOrigemId { get; set; }
        public virtual Conta ContaOrigem { get; set; }
        public Guid ContaDestinoId { get; set; }
        public virtual Conta ContaDestino { get; set; }
    }
}