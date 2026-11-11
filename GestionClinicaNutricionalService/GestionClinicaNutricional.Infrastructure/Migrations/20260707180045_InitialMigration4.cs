using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluacion_PlanAlimenticio_PlanAlimenticioId",
                table: "Evaluacion");

            migrationBuilder.DropForeignKey(
                name: "FK_Paciente_PlanAlimenticio_PlanAlimenticioId",
                table: "Paciente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanAlimenticio",
                table: "PlanAlimenticio");

            migrationBuilder.DropIndex(
                name: "IX_Paciente_PlanAlimenticioId",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "PlanAlimenticioId",
                table: "Paciente");

            migrationBuilder.RenameColumn(
                name: "PlanAlimenticioId",
                table: "PlanAlimenticio",
                newName: "PacienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanAlimenticio",
                table: "PlanAlimenticio",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlanAlimenticio_PacienteId",
                table: "PlanAlimenticio",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluacion_PlanAlimenticio_PlanAlimenticioId",
                table: "Evaluacion",
                column: "PlanAlimenticioId",
                principalTable: "PlanAlimenticio",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluacion_PlanAlimenticio_PlanAlimenticioId",
                table: "Evaluacion");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanAlimenticio_Paciente_PacienteId",
                table: "PlanAlimenticio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanAlimenticio",
                table: "PlanAlimenticio");

            migrationBuilder.DropIndex(
                name: "IX_PlanAlimenticio_PacienteId",
                table: "PlanAlimenticio");

            migrationBuilder.RenameColumn(
                name: "PacienteId",
                table: "PlanAlimenticio",
                newName: "PlanAlimenticioId");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanAlimenticioId",
                table: "Paciente",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanAlimenticio",
                table: "PlanAlimenticio",
                column: "PlanAlimenticioId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_PlanAlimenticioId",
                table: "Paciente",
                column: "PlanAlimenticioId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluacion_PlanAlimenticio_PlanAlimenticioId",
                table: "Evaluacion",
                column: "PlanAlimenticioId",
                principalTable: "PlanAlimenticio",
                principalColumn: "PlanAlimenticioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Paciente_PlanAlimenticio_PlanAlimenticioId",
                table: "Paciente",
                column: "PlanAlimenticioId",
                principalTable: "PlanAlimenticio",
                principalColumn: "PlanAlimenticioId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
