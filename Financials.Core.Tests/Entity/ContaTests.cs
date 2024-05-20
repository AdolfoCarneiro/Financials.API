using Financials.Core.Entity;
using Financials.Core.Enums;

namespace Financials.Core.Tests.Entity
{
    [TestFixture]
    [Category("UnitTests")]
    internal class ContaTests
    {
        [Test]
        public void Saldo_InicialSemTransacoesOuTransferencias_RetornaSaldoInicial()
        {
            var conta = new Conta
            {
                SaldoInicial = 1000
            };
            var saldo = conta.Saldo;

            Assert.That(saldo, Is.EqualTo(1000));
        }

        [Test]
        public void Saldo_ComReceitas_RetornaSaldoAtualizado()
        {
            var conta = new Conta
            {
                SaldoInicial = 1000,
                Transacoes = new List<Transacao>
                {
                    new Transacao { Valor = 200, Tipo = TipoTransacao.Receita },
                    new Transacao { Valor = 300, Tipo = TipoTransacao.Receita }
                }
            };

            var saldo = conta.Saldo;

            Assert.That(saldo, Is.EqualTo(1500));
        }

        [Test]
        public void Saldo_ComDespesas_RetornaSaldoAtualizado()
        {
            var conta = new Conta
            {
                SaldoInicial = 1000,
                Transacoes = new List<Transacao>
                {
                    new Transacao { Valor = 200, Tipo = TipoTransacao.Despesa },
                    new Transacao { Valor = 300, Tipo = TipoTransacao.Despesa }
                }
            };

            var saldo = conta.Saldo;

            Assert.That(saldo, Is.EqualTo(500));
        }

        [Test]
        public void Saldo_ComTransferenciasEnviadas_RetornaSaldoAtualizado()
        {
            var conta = new Conta
            {
                SaldoInicial = 1000,
                TransferenciasEnviadas = new List<Transferencia>
                {
                    new Transferencia { Valor = 200 },
                    new Transferencia { Valor = 300 }
                }
            };

            var saldo = conta.Saldo;

            Assert.That(saldo, Is.EqualTo(500));
        }

        [Test]
        public void Saldo_ComTransferenciasRecebidas_RetornaSaldoAtualizado()
        {
            var conta = new Conta
            {
                SaldoInicial = 1000,
                TransferenciasRecebidas = new List<Transferencia>
                {
                    new Transferencia { Valor = 200 },
                    new Transferencia { Valor = 300 }
                }
            };

            var saldo = conta.Saldo;

            Assert.That(saldo, Is.EqualTo(1500));
        }

        [Test]
        public void Saldo_ComTransacoesETransferencias_RetornaSaldoAtualizado()
        {
            var conta = new Conta
            {
                SaldoInicial = 1000,
                Transacoes = new List<Transacao>
                {
                    new Transacao { Valor = 200, Tipo = TipoTransacao.Receita },
                    new Transacao { Valor = 100, Tipo = TipoTransacao.Despesa }
                },
                TransferenciasEnviadas = new List<Transferencia>
                {
                    new Transferencia { Valor = 150 }
                },
                TransferenciasRecebidas = new List<Transferencia>
                {
                    new Transferencia { Valor = 250 }
                }
            };

            var saldo = conta.Saldo;

            Assert.That(saldo, Is.EqualTo(1200));
        }
    }
}
