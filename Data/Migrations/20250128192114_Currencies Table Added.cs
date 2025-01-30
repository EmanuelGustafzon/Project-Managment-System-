using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CurrenciesTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_CurrencyEntity_CurrencyId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrencyEntity",
                table: "CurrencyEntity");

            migrationBuilder.RenameTable(
                name: "CurrencyEntity",
                newName: "Currencies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Currencies_CurrencyId",
                table: "Services",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Currencies_CurrencyId",
                table: "Services");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "CurrencyEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrencyEntity",
                table: "CurrencyEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_CurrencyEntity_CurrencyId",
                table: "Services",
                column: "CurrencyId",
                principalTable: "CurrencyEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
