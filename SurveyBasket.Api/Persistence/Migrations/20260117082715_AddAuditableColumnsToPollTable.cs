using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableColumnsToPollTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateById",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateOn",
                table: "Polls",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateById",
                table: "Polls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateOn",
                table: "Polls",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_CreateById",
                table: "Polls",
                column: "CreateById");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_UpdateById",
                table: "Polls",
                column: "UpdateById");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Users_CreateById",
                table: "Polls",
                column: "CreateById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_Users_UpdateById",
                table: "Polls",
                column: "UpdateById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Users_CreateById",
                table: "Polls");

            migrationBuilder.DropForeignKey(
                name: "FK_Polls_Users_UpdateById",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_CreateById",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_UpdateById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "CreateById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "CreateOn",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "UpdateById",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "UpdateOn",
                table: "Polls");
        }
    }
}
