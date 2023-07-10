using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shipping.Migrations
{
    /// <inheritdoc />
    public partial class SeedingAdminPermissionsForCountOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13551064-d2ea-40ce-aad3-247a8622e38a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16da2458-b241-456b-924b-e971e04407cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "177b6a7c-4de3-4254-96fd-dab1dbc4f868");

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ArabicName", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[] { 5, "صلاحيات عدد الطلبات", "Permissions", "Permissions.OrderCount.View", "5ab58670-8727-4b67-85d5-4199912a70bf" });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ab58670-8727-4b67-85d5-4199912a70bf",
                column: "Date",
                value: "10/07/2023 08:12:04 م");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Date", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6b76e677-ddf6-4233-8fa7-c7079172bd23", null, "10/07/2023 08:12:04 م", "التجار", "التجار" },
                    { "8b89350b-cb59-4ae1-be39-be970cedaf34", null, "10/07/2023 08:12:04 م", "الموظفين", "الموظفين" },
                    { "9013509a-719b-41ba-8c77-6ccfd8688d17", null, "10/07/2023 08:12:04 م", "المناديب", "المناديب" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "76f86073-b51c-47c4-b7fa-731628055ebb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7bc4cab0-5aaa-48ff-8137-e8d8e10c8de4", "AQAAAAIAAYagAAAAEAwT5/ZNmmrKPy9btlT1n+O2H4LWDJc/YZDiiZ6/qioDFs4XWWqn2yWSFljCR//PJA==", "f92fb942-a31c-4580-9dc1-cb4ced5ddc60" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6b76e677-ddf6-4233-8fa7-c7079172bd23");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b89350b-cb59-4ae1-be39-be970cedaf34");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9013509a-719b-41ba-8c77-6ccfd8688d17");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ab58670-8727-4b67-85d5-4199912a70bf",
                column: "Date",
                value: "08/07/2023 06:42:11 م");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Date", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13551064-d2ea-40ce-aad3-247a8622e38a", null, "08/07/2023 06:42:11 م", "التجار", "التجار" },
                    { "16da2458-b241-456b-924b-e971e04407cb", null, "08/07/2023 06:42:11 م", "المناديب", "المناديب" },
                    { "177b6a7c-4de3-4254-96fd-dab1dbc4f868", null, "08/07/2023 06:42:11 م", "الموظفين", "الموظفين" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "76f86073-b51c-47c4-b7fa-731628055ebb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b3be9dbf-3dae-4e06-a12d-b0b9096752cf", "AQAAAAIAAYagAAAAENhoxyCsRNE/GCFMf0iLd6pXejkogtZchq3Y7f/QVthw0e2FAOGsWYq5jf8typHgqA==", "f2693b15-699a-40f9-95e6-4012d550c52e" });
        }
    }
}
