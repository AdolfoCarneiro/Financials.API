using Financials.Core.Enums;
using Financials.Services.RequestsResponses.Transacao.Validators;
using Financials.Services.RequestsResponses.Transacao;
using FluentValidation.TestHelper;

namespace Financials.Services.Tests.Services.Transacao.Validators
{
    [TestFixture]
    [Category("UnitTests")]
    public class RegristrarTransacaoRequestValidatorTests
    {
        private RegristrarTransacaoRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new RegristrarTransacaoRequestValidator();
        }

        [Test]
        public void Deve_Haver_Erro_Se_Valor_For_Zero()
        {
            var request = new RegristrarTransacaoRequest { Valor = 0 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Valor);
        }

        [Test]
        public void Deve_Haver_Erro_Se_Descricao_For_Nula_Ou_Vazia()
        {
            var request = new RegristrarTransacaoRequest { Descricao = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Descricao);
        }

        [Test]
        public void Deve_Haver_Erro_Se_CategoriaId_For_Vazia()
        {
            var request = new RegristrarTransacaoRequest { CategoriaId = Guid.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.CategoriaId);
        }

        [Test]
        public void Deve_Haver_Erro_Se_Data_For_Zero()
        {
            var request = new RegristrarTransacaoRequest { Data = default };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r.Data);
        }

        [Test]
        public void Deve_Haver_Erro_Se_Nao_Houver_ContaId_Ou_CartaoCreditoId()
        {
            var request = new RegristrarTransacaoRequest
            {
                ContaId = null,
                CartaoCreditoId = null
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(r => r);
        }

        [Test]
        public void Deve_Passar_Se_Houver_ContaId_Ou_CartaoCreditoId()
        {
            var request = new RegristrarTransacaoRequest
            {
                ContaId = Guid.NewGuid(),
                CartaoCreditoId = null
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(r => r.ContaId);
        }

        [Test]
        public void Deve_Passar_Se_Todos_Os_Campos_Forem_Validos()
        {
            var request = new RegristrarTransacaoRequest
            {
                Valor = 100,
                Descricao = "Teste",
                Data = DateTime.Now,
                Tipo = TipoTransacao.Despesa,
                ContaId = Guid.NewGuid(),
                CategoriaId = Guid.NewGuid(),
                CartaoCreditoId = null,
                FaturaId = null,
                Recorrente = false,
                FrequenciaRecorrencia = FrequenciaRecorrencia.Mensal,
                TotalParcelas = 1
            };
            var result = _validator.TestValidate(request);
            Assert.That(result.IsValid,Is.True);
        }
    }
}
