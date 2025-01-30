using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CurrencyEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_CurrencyId",
                table: "Services",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_CurrencyEntity_CurrencyId",
                table: "Services",
                column: "CurrencyId",
                principalTable: "CurrencyEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_CurrencyEntity_CurrencyId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "CurrencyEntity");

            migrationBuilder.DropIndex(
                name: "IX_Services_CurrencyId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Services");
        }
    }
}
