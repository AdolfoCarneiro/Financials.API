using Financials.Core.Entity;
using Financials.Services.Features.Account;
using Financials.Services.Interfaces.Account;
using Financials.Services.RequestsResponses.Account;
using Financials.Services.RequestsResponses.Base;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework.Legacy;

namespace Financials.Tests.Services.Account
{
    [TestFixture]
    public class CriarUsuarioTests
    {
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private Mock<IValidator<UsuarioRequest>> _usuarioRequestValidatorMock;

        [SetUp]
        public void SetUp()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
            _usuarioRequestValidatorMock = new Mock<IValidator<UsuarioRequest>>();
        }

        [Test]
        public async Task Run_Deve_Retornar_Application_response()
        {
            var request = new UsuarioRequest();
            var criarUsuario = new CriarUsuario(_userManagerMock.Object,_usuarioRequestValidatorMock.Object,_roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.That(result, Is.InstanceOf<ApplicationResponse<UsuarioResponse>>());
        }

        [Test]
        public async Task Run_Deve_Retornar_Erros_Quando_Request_Invalido()
        {
            var request = new UsuarioRequest();
            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new("Nome", "Nome é obrigatório") }));

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Does.Contain("Nome é obrigatório"));
            });
        }

        [Test]
        public async Task Run_Deve_Criar_Usuario_Quando_Request_Valido()
        {
            var request = new UsuarioRequest { Email = "email@example.com", Senha = "Password123!", Nome = "Nome", DataNascimento = DateTime.Now, Telefone = "123456789", Roles = ["Role1"] };

            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.That(result.Valid, Is.True);
            _userManagerMock.Verify(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _roleManagerMock.Verify(rm => rm.RoleExistsAsync(It.IsAny<string>()), Times.AtLeastOnce);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Test]
        public async Task Run_Deve_Retornar_Erros_Quando_UserManager_Falha()
        {
            var request = new UsuarioRequest { Email = "email@example.com", Senha = "Password123!", Nome = "Nome", DataNascimento = DateTime.Now, Telefone = "123456789", Roles = ["Role1"] };

            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Falha na criação do usuário" }));

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Does.Contain("Falha na criação do usuário"));
            });
        }

        [TestCase("NonExistingRole")]
        public async Task Run_Deve_Retornar_Erro_Quando_Role_Nao_Existe(string role)
        {
            var request = new UsuarioRequest
            {
                Email = "email@example.com",
                Senha = "Password123!",
                Nome = "Nome",
                DataNascimento = DateTime.Now,
                Telefone = "123456789",
                Roles = [role]
            };

            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(role))
                .ReturnsAsync(false);

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Does.Contain($"A role informada({role}) não foi encontrada"));
            });
        }

        [Test]
        public async Task Run_Deve_Retornar_Erro_Se_Ocorrer_Excecao()
        {
            var request = new UsuarioRequest {  };
            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Erro ao adicionar role"));

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);
            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Does.Contain("Ocorreu um erro ao criar a usuário"));
                Assert.That(result.Error.InternalError, Is.Not.Null);
            });
        }

        [Test]
        public async Task Run_Deve_Retornar_Erro_Quando_Usuario_Ja_Existir()
        {
            var request = new UsuarioRequest {};
            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Usuário já existe." }));

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Does.Contain("Usuário já existe."));
            });
        }

        [Test]
        public async Task Run_Deve_Adicionar_Usuario_As_Roles_Se_Criacao_For_Sucesso()
        {
            var request = new UsuarioRequest
            {
                Email = "email@example.com",
                Senha = "Password123!",
                Nome = "Nome",
                DataNascimento = DateTime.Now,
                Telefone = "123456789",
                Roles = ["Admin", "Default", "Suporte"]
            };
            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.That(result.Valid, Is.True);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Exactly(request.Roles.Count));
        }

        [Test]
        public async Task Run_Deve_Retornar_Erro_Se_RoleManager_Lancar_Excecao()
        {
            var request = new UsuarioRequest
            {
                Email = "email@example.com",
                Senha = "Password123!",
                Nome = "Nome",
                DataNascimento = DateTime.Now,
                Telefone = "123456789",
                Roles = ["Admin", "Default", "Suporte"]
            };

            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Erro ao adicionar usuário a role." }));
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);

            var result = await criarUsuario.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.CustomMessage, Does.Contain("Erro ao adicionar usuário a role."));
            });
        }

        [Test]
        public async Task Run_Deve_Excluir_Usuario_Se_Ocorrer_Excecao_Apos_Criacao()
        {
            var request = new UsuarioRequest
            {
                Email = "email@example.com",
                Senha = "Password123!",
                Nome = "Nome",
                DataNascimento = DateTime.Now,
                Telefone = "123456789",
                Roles = ["Admin", "Default", "Suporte"]
            };
            var createdUser = new ApplicationUser() { Email = request.Email};

            _usuarioRequestValidatorMock.Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<ApplicationUser, string>((user, _) => createdUser = user);
            _userManagerMock.Setup(um => um.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Erro ao adicionar role"));
            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var criarUsuario = new CriarUsuario(_userManagerMock.Object, _usuarioRequestValidatorMock.Object, _roleManagerMock.Object);
            var result = await criarUsuario.Run(request);

            Assert.Multiple(() =>
            {
                Assert.That(result.Valid, Is.False);
                Assert.That(result.Error.InternalError, Does.Contain("Erro ao adicionar role"));
            });

            _userManagerMock.Verify(um => um.DeleteAsync(createdUser), Times.Once);
        }

    }
}
