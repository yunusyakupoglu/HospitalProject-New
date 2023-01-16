using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Titles",
                columns: new[] { "Id", "PersonTitle", "Status" },
                values: new object[] { 1, "Sekreter", true });

            migrationBuilder.InsertData(
                table: "Titles",
                columns: new[] { "Id", "PersonTitle", "Status" },
                values: new object[] { 2, "Doktor", true });

            migrationBuilder.InsertData(
                table: "Titles",
                columns: new[] { "Id", "PersonTitle", "Status" },
                values: new object[] { 3, "Hasta", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Titles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Titles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Titles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
