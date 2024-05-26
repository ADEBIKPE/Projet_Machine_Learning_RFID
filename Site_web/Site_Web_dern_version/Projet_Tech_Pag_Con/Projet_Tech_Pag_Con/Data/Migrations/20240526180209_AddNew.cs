using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_Tech_Pag_Con.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionMethode_Simulation_SimulationId",
                table: "ExecutionMethode");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionMethode_Simulation_SimulationId1",
                table: "ExecutionMethode");

            migrationBuilder.DropIndex(
                name: "IX_ExecutionMethode_SimulationId1",
                table: "ExecutionMethode");

            migrationBuilder.DropColumn(
                name: "SimulationId1",
                table: "ExecutionMethode");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionMethode_Simulation_SimulationId",
                table: "ExecutionMethode",
                column: "SimulationId",
                principalTable: "Simulation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionMethode_Simulation_SimulationId",
                table: "ExecutionMethode");

            migrationBuilder.AddColumn<int>(
                name: "SimulationId1",
                table: "ExecutionMethode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionMethode_SimulationId1",
                table: "ExecutionMethode",
                column: "SimulationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionMethode_Simulation_SimulationId",
                table: "ExecutionMethode",
                column: "SimulationId",
                principalTable: "Simulation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionMethode_Simulation_SimulationId1",
                table: "ExecutionMethode",
                column: "SimulationId1",
                principalTable: "Simulation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
