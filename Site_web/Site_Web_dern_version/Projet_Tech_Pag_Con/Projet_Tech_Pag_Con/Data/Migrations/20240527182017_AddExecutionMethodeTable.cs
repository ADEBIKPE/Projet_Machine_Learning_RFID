using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_Tech_Pag_Con.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExecutionMethodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionMethodesAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomMethode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Performance = table.Column<float>(type: "real", nullable: false),
                    MatriceConfusion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temps_Execution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRoleId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionMethodesAdmin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Simulation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSimulation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtilisateurId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Simulation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Simulation_AspNetUsers_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SimulationAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateSimulation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtilisateurId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimulationAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimulationAdmin_AspNetUsers_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionMethode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomMethode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Performance = table.Column<float>(type: "real", nullable: false),
                    MatriceConfusion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temps_Execution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SimulationId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserRoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionMethode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionMethode_AspNetRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionMethode_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionMethode_Simulation_SimulationId",
                        column: x => x.SimulationId,
                        principalTable: "Simulation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionMethode_SimulationId",
                table: "ExecutionMethode",
                column: "SimulationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionMethode_UserId",
                table: "ExecutionMethode",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionMethode_UserRoleId",
                table: "ExecutionMethode",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Simulation_UtilisateurId",
                table: "Simulation",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_SimulationAdmin_UtilisateurId",
                table: "SimulationAdmin",
                column: "UtilisateurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionMethode");

            migrationBuilder.DropTable(
                name: "ExecutionMethodesAdmin");

            migrationBuilder.DropTable(
                name: "SimulationAdmin");

            migrationBuilder.DropTable(
                name: "Simulation");
        }
    }
}
