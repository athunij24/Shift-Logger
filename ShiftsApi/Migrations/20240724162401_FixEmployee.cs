using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftsApi.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "Shifts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
