using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "36093881-ce87-4393-b987-6d6d5fec918c");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405", "90545647-1413-4dcd-854f-e78d91af5765" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "90545647-1413-4dcd-854f-e78d91af5765");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 14,
                column: "RoleId",
                value: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDelete", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "019c5391-6d22-7b62-a1fc-86670bf948e7", "019c5391-6d22-7b62-a1fc-866888073330", false, false, "Admin", "ADMIN" },
                    { "019c5391-6d22-7b62-a1fc-8669732cfe25", "019c5391-6d22-7b62-a1fc-866a5d070ff7", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "019c5391-6d22-7b62-a1fc-866b9b85d123", 0, "019c5391-6d22-7b62-a1fc-866c586d645b", "admin@survey-basket.com", true, "admin", false, "surveybasket", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEK9Mol/ShfWrLeIDrzSoxaloIZehVrrkzqg0c3Ofl6QYnMqEDhBb6ICuGTtb9Ugw5A==", null, false, "F9FE818E0C5C4DDF98FF63FDFE7359E9", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "019c5391-6d22-7b62-a1fc-86670bf948e7", "019c5391-6d22-7b62-a1fc-866b9b85d123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "019c5391-6d22-7b62-a1fc-8669732cfe25");

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "019c5391-6d22-7b62-a1fc-86670bf948e7", "019c5391-6d22-7b62-a1fc-866b9b85d123" });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "019c5391-6d22-7b62-a1fc-86670bf948e7");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "019c5391-6d22-7b62-a1fc-866b9b85d123");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.UpdateData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 14,
                column: "RoleId",
                value: "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDelete", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "36093881-ce87-4393-b987-6d6d5fec918c", "86afec26-74bc-4ff7-b573-960d2b59b32f", true, false, "Member", "MEMBER" },
                    { "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405", "756d20e0-2d5b-4e2e-999b-2d614b985c06", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "90545647-1413-4dcd-854f-e78d91af5765", 0, "dabdf641-4ad0-48a1-819d-98ec546e0856", "admin@survey-basket.com", true, "admin", false, "surveybasket", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEDn7U/HKFDgd1deEGkwpXK3cHSyBMA9hcXQ3Rs6VwiHDmy8BDGCVakX+mDrpBX0qSA==", null, false, "F9FE818E0C5C4DDF98FF63FDFE7359E9", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405", "90545647-1413-4dcd-854f-e78d91af5765" });
        }
    }
}
