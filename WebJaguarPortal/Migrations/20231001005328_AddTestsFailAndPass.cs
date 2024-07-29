using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebJaguarPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddTestsFailAndPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestsFail",
                table: "ControlFlowAnalyzes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TestsPass",
                table: "ControlFlowAnalyzes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestsFail",
                table: "ControlFlowAnalyzes");

            migrationBuilder.DropColumn(
                name: "TestsPass",
                table: "ControlFlowAnalyzes");
        }
    }
}
