using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiftLog.Migrations
{
    /// <inheritdoc />
    public partial class DataAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "MuscleGroup", "Name" },
                values: new object[] { 3, "Back", "Deadlift" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
