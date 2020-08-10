using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class apiMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Adiantamento",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<Guid>(nullable: false),
                    DataRequisicao = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataPagamentoInicio = table.Column<DateTime>(nullable: false),
                    DataPagamentoFim = table.Column<DateTime>(nullable: false),
                    DataRepasse = table.Column<bool>(nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    NetAmount = table.Column<decimal>(nullable: false),
                    TotalRepasse = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    FixedTaxes = table.Column<decimal>(nullable: false),
                    AdvanceTaxes = table.Column<decimal>(nullable: false),
                    AvaliadoPor = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adiantamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adiantamento_Cliente_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adiantamento_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Result = table.Column<int>(nullable: false),
                    DataPagamento = table.Column<DateTime>(nullable: false),
                    ConfirmacaoAdquirente = table.Column<int>(nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    DataRepasse = table.Column<DateTime>(nullable: true),
                    InstalmentsCount = table.Column<short>(nullable: false),
                    NomeCartao = table.Column<string>(type: "varchar(255)", nullable: true),
                    ExpiracaoCartao = table.Column<string>(type: "varchar(7)", nullable: true),
                    CardSecurityNumber = table.Column<string>(nullable: true),
                    CardDigits = table.Column<string>(nullable: true),
                    DigitosCartao = table.Column<short>(type: "smallint", nullable: false),
                    AdvanceId1 = table.Column<long>(nullable: true),
                    AdvanceId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamento_Adiantamento_AdvanceId",
                        column: x => x.AdvanceId,
                        principalTable: "Adiantamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamento_Adiantamento_AdvanceId1",
                        column: x => x.AdvanceId1,
                        principalTable: "Adiantamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamento_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parcela",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdCliente = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPagamento = table.Column<long>(type: "bigint", nullable: false),
                    ValorTotalParcela = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    AllInstallments = table.Column<decimal>(nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime", nullable: true),
                    NumParcela = table.Column<short>(type: "smallint", nullable: false),
                    NumTotalParcelas = table.Column<short>(type: "smallint", nullable: false),
                    CriadoEm = table.Column<DateTime>(nullable: true),
                    DataAlvoPagamento = table.Column<DateTime>(type: "datetime", nullable: false),
                    TaxaFixa = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    TaxaAntecipacao = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcela", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcela_Cliente_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parcela_Pagamento_IdPagamento",
                        column: x => x.IdPagamento,
                        principalTable: "Pagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adiantamento_CustomerId",
                table: "Adiantamento",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Adiantamento_DataPagamentoFim",
                table: "Adiantamento",
                column: "DataPagamentoFim");

            migrationBuilder.CreateIndex(
                name: "IX_Adiantamento_DataRequisicao",
                table: "Adiantamento",
                column: "DataRequisicao");

            migrationBuilder.CreateIndex(
                name: "IX_Adiantamento_UserId",
                table: "Adiantamento",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_AdvanceId",
                table: "Pagamento",
                column: "AdvanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_AdvanceId1",
                table: "Pagamento",
                column: "AdvanceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_DataPagamento",
                table: "Pagamento",
                column: "DataPagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_IdCliente",
                table: "Pagamento",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_DataRepasse",
                table: "Pagamento",
                column: "DataRepasse");

            migrationBuilder.CreateIndex(
                name: "IX_Parcela_IdCliente",
                table: "Parcela",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Parcela_IdPagamento",
                table: "Parcela",
                column: "IdPagamento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcela");

            migrationBuilder.DropTable(
                name: "Pagamento");

            migrationBuilder.DropTable(
                name: "Adiantamento");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
