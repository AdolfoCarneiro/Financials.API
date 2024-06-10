using Financials.Core.Entity;
using Financials.Core.Enums;

namespace Financials.Core.Tests.Entity
{
    [TestFixture]
    [Category("UnitTests")]
    public class TransacaoTests
    {
        [Test]
        public void CalcularProximaData_FrequenciaDiaria_RetornaDataIncrementadaEm1Dia()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = FrequenciaRecorrencia.Diaria;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual.AddDays(1)));
        }

        [Test]
        public void CalcularProximaData_FrequenciaSemanal_RetornaDataIncrementadaEm7Dias()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = FrequenciaRecorrencia.Semanal;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual.AddDays(7)));
        }

        [Test]
        public void CalcularProximaData_FrequenciaQuinzenal_RetornaDataIncrementadaEm15Dias()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = FrequenciaRecorrencia.Quinzenal;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual.AddDays(15)));
        }

        [Test]
        public void CalcularProximaData_FrequenciaMensal_RetornaDataIncrementadaEm1Mes()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = FrequenciaRecorrencia.Mensal;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual.AddMonths(1)));
        }

        [Test]
        public void CalcularProximaData_FrequenciaBimestral_RetornaDataIncrementadaEm2Meses()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = FrequenciaRecorrencia.Bimestral;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual.AddMonths(2)));
        }

        [Test]
        public void CalcularProximaData_FrequenciaTrimestral_RetornaDataIncrementadaEm3Meses()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = FrequenciaRecorrencia.Trimestral;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual.AddMonths(3)));
        }

        [Test]
        public void CalcularProximaData_FrequenciaAnual_RetornaDataIncrementadaEm1Ano()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = FrequenciaRecorrencia.Anual;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual.AddYears(1)));
        }

        [Test]
        public void CalcularProximaData_FrequenciaDesconhecida_RetornaDataAtual()
        {
            var dataAtual = new DateTime(2024, 5, 20);
            var frequencia = (FrequenciaRecorrencia)999;

            var proximaData = Transacao.CalcularProximaData(dataAtual, frequencia);

            Assert.That(proximaData, Is.EqualTo(dataAtual));
        }
    }
}
