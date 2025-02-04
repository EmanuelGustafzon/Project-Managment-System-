using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class UsersAndCurrenciesTypeCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Currencies_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_CustomerEntity_CustomerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_CurrencyEntity_CurrencyId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Currencies_UserId",
                table: "UserContacts");

            migrationBuilder.DropTable(
                name: "CurrencyEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerEntity",
                table: "CustomerEntity");

            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "CustomerEntity",
                newName: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Currencies",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Firstname = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Customers_CustomerId",
                table: "Projects",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Currencies_CurrencyId",
                table: "Services",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Users_UserId",
                table: "UserContacts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Customers_CustomerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Currencies_CurrencyId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_Users_UserId",
                table: "UserContacts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Currencies");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "CustomerEntity");

            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "Currencies",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "Currencies",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerEntity",
                table: "CustomerEntity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CurrencyEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyEntity", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Currencies_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_CustomerEntity_CustomerId",
                table: "Projects",
                column: "CustomerId",
                principalTable: "CustomerEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_CurrencyEntity_CurrencyId",
                table: "Services",
                column: "CurrencyId",
                principalTable: "CurrencyEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_Currencies_UserId",
                table: "UserContacts",
                column: "UserId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
