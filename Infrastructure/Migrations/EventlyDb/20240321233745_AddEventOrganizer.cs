using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Data.Migrations.EventlyDb
{
    /// <inheritdoc />
    public partial class AddEventOrganizer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_UserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganizerId",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_UserId",
                table: "Events",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
