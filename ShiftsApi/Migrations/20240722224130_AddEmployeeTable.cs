using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Length",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Shifts");

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "Shifts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Employees_EmployeeId",
                table: "Shifts");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Shifts_EmployeeId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Shifts");

            migrationBuilder.AddColumn<string>(
                name: "Length",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Shifts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
