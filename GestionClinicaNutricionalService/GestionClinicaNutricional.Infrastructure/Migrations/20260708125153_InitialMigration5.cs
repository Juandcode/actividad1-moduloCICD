using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paciente_ConsultaInicial_ConsultaInicialId",
                table: "Paciente");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanAlimenticio_Paciente_PacienteId",
                table: "PlanAlimenticio");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_ConsultaInicialId",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "ConsultaInicialId",
                table: "Paciente");

            migrationBuilder.RenameColumn(
                name: "PacienteId",
                table: "PlanAlimenticio",
                newName: "ConsultaInicialId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanAlimenticio_PacienteId",
                table: "PlanAlimenticio",
                newName: "IX_PlanAlimenticio_ConsultaInicialId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha",
                table: "ConsultaInicial",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "PacienteId",
                table: "ConsultaInicial",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ConsultaInicial_PacienteId",
                table: "ConsultaInicial",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsultaInicial_Paciente_PacienteId",
                table: "ConsultaInicial",
                column: "PacienteId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "PlanAlimenticio",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsultaInicial_Paciente_PacienteId",
                table: "ConsultaInicial");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "PlanAlimenticio");

            migrationBuilder.DropIndex(
                name: "IX_ConsultaInicial_PacienteId",
                table: "ConsultaInicial");

            migrationBuilder.DropColumn(
                name: "Fecha",
                table: "ConsultaInicial");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "ConsultaInicial");

            migrationBuilder.RenameColumn(
                name: "ConsultaInicialId",
                table: "PlanAlimenticio",
                newName: "PacienteId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanAlimenticio_ConsultaInicialId",
                table: "PlanAlimenticio",
                newName: "IX_PlanAlimenticio_PacienteId");

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultaInicialId",
                table: "Paciente",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_ConsultaInicialId",
                table: "Paciente",
                column: "ConsultaInicialId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Paciente_ConsultaInicial_ConsultaInicialId",
                table: "Paciente",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanAlimenticio_Paciente_PacienteId",
                table: "PlanAlimenticio",
                column: "PacienteId",
                principalTable: "Paciente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
