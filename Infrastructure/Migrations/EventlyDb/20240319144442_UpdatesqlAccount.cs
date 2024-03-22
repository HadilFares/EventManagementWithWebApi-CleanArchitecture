using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations.EventlyDb
{
    /// <inheritdoc />
    public partial class UpdatesqlAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_UserId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_UserId",
                table: "Accounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AspNetUsers_UserId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AspNetUsers_UserId",
                table: "Accounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
