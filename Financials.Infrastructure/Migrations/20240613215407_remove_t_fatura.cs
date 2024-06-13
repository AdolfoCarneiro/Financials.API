using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Financials.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_t_fatura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacao_CartaoCredito_CartaoCreditoId",
                table: "Transacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacao_Conta_ContaId",
                table: "Transacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacao_Fatura_FaturaId",
                table: "Transacao");

            migrationBuilder.DropTable(
                name: "Fatura");

            migrationBuilder.DropIndex(
                name: "IX_Transacao_FaturaId",
                table: "Transacao");

            migrationBuilder.DropColumn(
                name: "FaturaId",
                table: "Transacao");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacao_CartaoCredito_CartaoCreditoId",
                table: "Transacao",
                column: "CartaoCreditoId",
                principalTable: "CartaoCredito",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacao_Conta_ContaId",
                table: "Transacao",
                column: "ContaId",
                principalTable: "Conta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacao_CartaoCredito_CartaoCreditoId",
                table: "Transacao");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacao_Conta_ContaId",
                table: "Transacao");

            migrationBuilder.AddColumn<Guid>(
                name: "FaturaId",
                table: "Transacao",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Fatura",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartaoCreditoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_FaturaId",
                table: "Transacao",
                column: "FaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Fatura_CartaoCreditoId",
                table: "Fatura",
                column: "CartaoCreditoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacao_CartaoCredito_CartaoCreditoId",
                table: "Transacao",
                column: "CartaoCreditoId",
                principalTable: "CartaoCredito",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacao_Conta_ContaId",
                table: "Transacao",
                column: "ContaId",
                principalTable: "Conta",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacao_Fatura_FaturaId",
                table: "Transacao",
                column: "FaturaId",
                principalTable: "Fatura",
                principalColumn: "Id");
        }
    }
}
