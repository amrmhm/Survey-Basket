using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "90545647-1413-4dcd-854f-e78d91af5765", 0, "dabdf641-4ad0-48a1-819d-98ec546e0856", "admin@survey-basket.com", true, "admin", "surveybasket", false, null, "ADMIN@SURVEY-BASKET.COM", "ADMIN@SURVEY-BASKET.COM", "AQAAAAIAAYagAAAAEK9Mol/ShfWrLeIDrzSoxaloIZehVrrkzqg0c3Ofl6QYnMqEDhBb6ICuGTtb9Ugw5A==", null, false, "F9FE818E0C5C4DDF98FF63FDFE7359E9", false, "admin@survey-basket.com" });

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permmisions", "polls:read", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 2, "Permmisions", "polls:add", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 3, "Permmisions", "polls:update", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 4, "Permmisions", "polls:delete", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 5, "Permmisions", "questions:read", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 6, "Permmisions", "questions:add", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 7, "Permmisions", "questions:update", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 8, "Permmisions", "users:read", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 9, "Permmisions", "users:add", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 10, "Permmisions", "users:update", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 11, "Permmisions", "roles:read", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 12, "Permmisions", "roles:add", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 13, "Permmisions", "roles:update", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" },
                    { 14, "Permmisions", "resaults:read", "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "a99c439c-1d8b-4ca1-9ec9-a7c1c668a405", "90545647-1413-4dcd-854f-e78d91af5765" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 14);

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
        }
    }
}
