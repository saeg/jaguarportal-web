using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebJaguarPortal.Migrations
{
    /// <inheritdoc />
    public partial class CreatePullRequestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "ControlFlowAnalyzes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PullRequestBase",
                table: "ControlFlowAnalyzes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PullRequestBranch",
                table: "ControlFlowAnalyzes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PullRequestNumber",
                table: "ControlFlowAnalyzes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Repository",
                table: "ControlFlowAnalyzes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Provider",
                table: "ControlFlowAnalyzes");

            migrationBuilder.DropColumn(
                name: "PullRequestBase",
                table: "ControlFlowAnalyzes");

            migrationBuilder.DropColumn(
                name: "PullRequestBranch",
                table: "ControlFlowAnalyzes");

            migrationBuilder.DropColumn(
                name: "PullRequestNumber",
                table: "ControlFlowAnalyzes");

            migrationBuilder.DropColumn(
                name: "Repository",
                table: "ControlFlowAnalyzes");
        }
    }
}
