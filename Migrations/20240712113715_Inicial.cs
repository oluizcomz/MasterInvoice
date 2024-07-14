using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterInvoice.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BillingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceDoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BillDoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payments_InvoiceId",
                table: "payments",
                column: "InvoiceId");


            // Inserindo os valores na tabela status
            migrationBuilder.Sql("INSERT INTO status(Id, Description) VALUES (1, 'Emitida');");
            migrationBuilder.Sql("INSERT INTO status(Id, Description) VALUES (2, 'Cobrança Realizada');");
            migrationBuilder.Sql("INSERT INTO status(Id, Description) VALUES (3, 'Pagamento em Atraso');");
            migrationBuilder.Sql("INSERT INTO status(Id, Description) VALUES (4, 'Pagamento Realizado');");

            // Adicionando a trigger para INSERT e UPDATE
            migrationBuilder.Sql(@"
                CREATE TRIGGER UpdateInvoiceStatusAfterPayment
                ON payments
                AFTER INSERT, UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    UPDATE invoices
                    SET StatusId = 4
                    FROM invoices
                    INNER JOIN inserted ON invoices.Id = inserted.InvoiceId;
                END
            ");


            // Inserindo registros de teste na tabela invoices 
            migrationBuilder.Sql(@"
                DECLARE @startDate DATE = DATEADD(MONTH, -12, GETDATE());
                DECLARE @endDate DATE = GETDATE();

                DECLARE @currentDate DATE = @startDate;
                WHILE @currentDate <= @endDate
                BEGIN
                    DECLARE @IdentificationNumber VARCHAR(50) = NEWID();

                    DECLARE @statusId INT = 1;
                    WHILE @statusId <= 3
                    BEGIN
                        INSERT INTO invoices (PayerName, IdentificationNumber, IssueDate,InvoiceDoc,BillDoc, Amount, StatusId)
                        VALUES
                        ('Empresa A', @IdentificationNumber, @currentDate,'xxx-xx', 'yyy-yy', RAND() * 100, @statusId);

                        SET @statusId = @statusId + 1;
                    END

                    SET @currentDate = DATEADD(MONTH, 1, @currentDate);
                END
            ");
            // payment
            migrationBuilder.Sql(@"
                DECLARE @startDate DATE = DATEADD(MONTH, -6, GETDATE());
                DECLARE @endDate DATE = GETDATE();

                DECLARE @currentDate DATE = @startDate;

                WHILE @currentDate <= @endDate
                BEGIN
                    INSERT INTO payments (InvoiceId, PaymentDate)
                    SELECT Id, DATEADD(DAY, 10, BillingDate) -- Assuming payment date is 10 days after billing date
                    FROM invoices
                    WHERE StatusId = 2
                    AND IssueDate <= @currentDate
                    AND @currentDate <= DATEADD(MONTH, 6, IssueDate);

                    SET @currentDate = DATEADD(MONTH, 1, @currentDate);
                END
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "status");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.Sql("DROP TRIGGER IF EXISTS UpdateInvoiceStatusAfterPayment");
        }
    }
}
