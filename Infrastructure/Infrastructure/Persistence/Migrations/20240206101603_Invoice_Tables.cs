using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Invoice_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    EconomicCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SendStatus = table.Column<int>(type: "int", nullable: false),
                    TotalPriceBeforDiscount = table.Column<long>(type: "bigint", nullable: false),
                    DiscountAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalPriceAfterDiscount = table.Column<long>(type: "bigint", nullable: false),
                    TaxAmount = table.Column<long>(type: "bigint", nullable: false),
                    OtherTaxAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount = table.Column<long>(type: "bigint", nullable: false),
                    CashAmount = table.Column<long>(type: "bigint", nullable: false),
                    CreditAmount = table.Column<long>(type: "bigint", nullable: false),
                    BuyerName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    BuyerNationalCode = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    BuyerEconomicCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BuyerMobile = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    BuyerAddress = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BuyerPostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BuyerPhone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    BuyerIsRealOrLegal = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    PayType = table.Column<int>(type: "int", nullable: false),
                    TaxInvoiceType = table.Column<int>(type: "int", nullable: false),
                    TaxInvoicePattern = table.Column<int>(type: "int", nullable: false),
                    TaxInvoiceSubject = table.Column<int>(type: "int", nullable: false),
                    SerialNumber = table.Column<long>(type: "bigint", nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaxUid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxRefNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxErrorCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxErrorDetail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TaxStatusMessage = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TaxPacketType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxFiscalId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxInquiryData = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<long>(type: "bigint", nullable: false),
                    TotalPriceBeforDiscount = table.Column<long>(type: "bigint", nullable: false),
                    DiscountAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalPriceAfterDiscount = table.Column<long>(type: "bigint", nullable: false),
                    TaxRate = table.Column<float>(type: "real", nullable: false),
                    TaxAmount = table.Column<long>(type: "bigint", nullable: false),
                    OtherTaxAmount = table.Column<long>(type: "bigint", nullable: false),
                    TotalPrice = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CompanyId",
                table: "Invoices",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
