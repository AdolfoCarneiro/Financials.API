﻿using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Account.Validators;
using FluentValidation.TestHelper;

namespace Financials.Tests.Services.Account.Validators
{
    [TestFixture]
    public class UsuarioResquestValidatorTests
    {
        private UsuarioRequestValidator _validator;
        [SetUp]
        public void Setup()
        {
            _validator = new UsuarioRequestValidator();
        }

        [TestCase("")]
        [TestCase("as")]
        public void Should_have_error_when_Name_is_Invalid(string nome)
        {
            var request = new UsuarioRequest()
            {
                Nome = nome
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Nome);
        }

        [TestCase("")]
        [TestCase("as")]
        public void Should_have_error_when_Senha_is_Invalid(string senha)
        {
            var request = new UsuarioRequest()
            {
                Senha = senha
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Senha);
        }

        [TestCase("")]
        [TestCase("as")]
        public void Should_have_error_when_Email_is_Invalid(string email)
        {
            var request = new UsuarioRequest()
            {
                Email = email
            };

            var validation = _validator.TestValidate(request);

            validation.ShouldHaveValidationErrorFor(usuario => usuario.Email);
        }
    }
}