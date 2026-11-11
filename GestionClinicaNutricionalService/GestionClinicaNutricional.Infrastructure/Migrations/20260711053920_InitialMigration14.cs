using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "PlanAlimenticio");

            migrationBuilder.RenameColumn(
                name: "ConsultaInicialId",
                table: "PlanAlimenticio",
                newName: "PacienteId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanAlimenticio_ConsultaInicialId",
                table: "PlanAlimenticio",
                newName: "IX_PlanAlimenticio_PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanAlimenticio_Paciente_PacienteId",
                table: "PlanAlimenticio",
                column: "PacienteId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanAlimenticio_Paciente_PacienteId",
                table: "PlanAlimenticio");

            migrationBuilder.RenameColumn(
                name: "PacienteId",
                table: "PlanAlimenticio",
                newName: "ConsultaInicialId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanAlimenticio_PacienteId",
                table: "PlanAlimenticio",
                newName: "IX_PlanAlimenticio_ConsultaInicialId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "PlanAlimenticio",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
