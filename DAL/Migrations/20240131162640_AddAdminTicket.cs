using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class AddAdminTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Tickets",
                newName: "IsActivated");

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "IsActivated", "SessionId", "UpdatedAt", "Value", "WeatherTariff" },
                values: new object[] { 1, new DateTime(2024, 1, 31, 16, 26, 40, 771, DateTimeKind.Utc).AddTicks(7867), false, null, new DateTime(2024, 1, 31, 16, 26, 40, 771, DateTimeKind.Utc).AddTicks(7870), "YXBGY4V78O", 1000 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameColumn(
                name: "IsActivated",
                table: "Tickets",
                newName: "IsActive");
        }
    }
}
