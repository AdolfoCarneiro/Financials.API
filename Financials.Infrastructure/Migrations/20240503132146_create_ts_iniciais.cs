using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financials.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class create_ts_iniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartaoCredito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Limite = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataFechamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartaoCredito", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fatura",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CartaoCreditoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fatura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fatura_CartaoCredito_CartaoCreditoId",
                        column: x => x.CartaoCreditoId,
                        principalTable: "CartaoCredito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transferencia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContaOrigemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContaDestinoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transferencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transferencia_Conta_ContaDestinoId",
                        column: x => x.ContaDestinoId,
                        principalTable: "Conta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transferencia_Conta_ContaOrigemId",
                        column: x => x.ContaOrigemId,
                        principalTable: "Conta",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transacao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    ContaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CategoriaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartaoCreditoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FaturaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Recorrente = table.Column<bool>(type: "bit", nullable: false),
                    FrequenciaRecorrencia = table.Column<int>(type: "int", nullable: false),
                    TotalParcelas = table.Column<int>(type: "int", nullable: false),
                    NumeroParcela = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacao_CartaoCredito_CartaoCreditoId",
                        column: x => x.CartaoCreditoId,
                        principalTable: "CartaoCredito",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transacao_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacao_Conta_ContaId",
                        column: x => x.ContaId,
                        principalTable: "Conta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transacao_Fatura_FaturaId",
                        column: x => x.FaturaId,
                        principalTable: "Fatura",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fatura_CartaoCreditoId",
                table: "Fatura",
                column: "CartaoCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_CartaoCreditoId",
                table: "Transacao",
                column: "CartaoCreditoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_CategoriaId",
                table: "Transacao",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_ContaId",
                table: "Transacao",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_FaturaId",
                table: "Transacao",
                column: "FaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Transferencia_ContaDestinoId",
                table: "Transferencia",
                column: "ContaDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transferencia_ContaOrigemId",
                table: "Transferencia",
                column: "ContaOrigemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacao");

            migrationBuilder.DropTable(
                name: "Transferencia");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Fatura");

            migrationBuilder.DropTable(
                name: "Conta");

            migrationBuilder.DropTable(
                name: "CartaoCredito");
        }
    }
}
