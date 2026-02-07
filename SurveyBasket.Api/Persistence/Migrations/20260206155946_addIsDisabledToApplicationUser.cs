using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addIsDisabledToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "90545647-1413-4dcd-854f-e78d91af5765",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEDn7U/HKFDgd1deEGkwpXK3cHSyBMA9hcXQ3Rs6VwiHDmy8BDGCVakX+mDrpBX0qSA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "90545647-1413-4dcd-854f-e78d91af5765",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEIjxqVqgGoLqwpKHJ0E9h5X5fS4jl+aXjUsIEBAjUuGxTwAeXHnvDvwCPnu9T+/AQQ==");
        }
    }
}
