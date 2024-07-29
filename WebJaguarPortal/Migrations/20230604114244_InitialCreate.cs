using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebJaguarPortal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileAnalyzes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Hash = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAnalyzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RenewPasswords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewPasswords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SmtpAddress = table.Column<string>(type: "text", nullable: true),
                    SmtpFrom = table.Column<string>(type: "text", nullable: true),
                    SmtpUsername = table.Column<string>(type: "text", nullable: true),
                    SmtpPassword = table.Column<string>(type: "text", nullable: true),
                    SmtpUseSSL = table.Column<bool>(type: "boolean", nullable: false),
                    SmtpPort = table.Column<int>(type: "integer", nullable: true),
                    JWTSigningKey = table.Column<string>(type: "text", nullable: false),
                    EntropyLevelPassword = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    List = table.Column<bool>(type: "boolean", nullable: false),
                    Detail = table.Column<bool>(type: "boolean", nullable: false),
                    Edit = table.Column<bool>(type: "boolean", nullable: false),
                    Delete = table.Column<bool>(type: "boolean", nullable: false),
                    New = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ControlFlowAnalyzes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartAnalysis = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndAnalysis = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MessageError = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlFlowAnalyzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlFlowAnalyzes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    UsersPermissionId = table.Column<long>(type: "bigint", nullable: true),
                    AnalyzesPermissionId = table.Column<long>(type: "bigint", nullable: true),
                    ProjectsPermissionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_UserPermissions_AnalyzesPermissionId",
                        column: x => x.AnalyzesPermissionId,
                        principalTable: "UserPermissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_UserPermissions_ProjectsPermissionId",
                        column: x => x.ProjectsPermissionId,
                        principalTable: "UserPermissions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserRoles_UserPermissions_UsersPermissionId",
                        column: x => x.UsersPermissionId,
                        principalTable: "UserPermissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassAnalyzes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnalysisId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    FileAnalyzeId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassAnalyzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassAnalyzes_ControlFlowAnalyzes_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "ControlFlowAnalyzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassAnalyzes_FileAnalyzes_FileAnalyzeId",
                        column: x => x.FileAnalyzeId,
                        principalTable: "FileAnalyzes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    ClientSecret = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RolesId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "UserRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LineAnalyzes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassAnalysisId = table.Column<long>(type: "bigint", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false),
                    NumberLine = table.Column<int>(type: "integer", nullable: false),
                    Cef = table.Column<int>(type: "integer", nullable: false),
                    Cep = table.Column<int>(type: "integer", nullable: false),
                    Cnf = table.Column<int>(type: "integer", nullable: false),
                    Cnp = table.Column<int>(type: "integer", nullable: false),
                    SuspiciousValue = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineAnalyzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineAnalyzes_ClassAnalyzes_ClassAnalysisId",
                        column: x => x.ClassAnalysisId,
                        principalTable: "ClassAnalyzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassAnalyzes_AnalysisId",
                table: "ClassAnalyzes",
                column: "AnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassAnalyzes_FileAnalyzeId",
                table: "ClassAnalyzes",
                column: "FileAnalyzeId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlFlowAnalyzes_ProjectId",
                table: "ControlFlowAnalyzes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LineAnalyzes_ClassAnalysisId",
                table: "LineAnalyzes",
                column: "ClassAnalysisId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_AnalyzesPermissionId",
                table: "UserRoles",
                column: "AnalyzesPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_ProjectsPermissionId",
                table: "UserRoles",
                column: "ProjectsPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UsersPermissionId",
                table: "UserRoles",
                column: "UsersPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolesId",
                table: "Users",
                column: "RolesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineAnalyzes");

            migrationBuilder.DropTable(
                name: "RenewPasswords");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ClassAnalyzes");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ControlFlowAnalyzes");

            migrationBuilder.DropTable(
                name: "FileAnalyzes");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
