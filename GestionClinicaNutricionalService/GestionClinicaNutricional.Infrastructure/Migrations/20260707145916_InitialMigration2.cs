using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "HabitoAlimenticio");

            migrationBuilder.DropForeignKey(
                name: "FK_Paciente_ConsultaInicial_ConsultaInicialId",
                table: "Paciente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsultaInicial",
                table: "ConsultaInicial");

            migrationBuilder.DropColumn(
                name: "ConsultaInicialId",
                table: "ConsultaInicial");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsultaInicial",
                table: "ConsultaInicial",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "HabitoAlimenticio",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Paciente_ConsultaInicial_ConsultaInicialId",
                table: "Paciente",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "HabitoAlimenticio");

            migrationBuilder.DropForeignKey(
                name: "FK_Paciente_ConsultaInicial_ConsultaInicialId",
                table: "Paciente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsultaInicial",
                table: "ConsultaInicial");

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultaInicialId",
                table: "ConsultaInicial",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsultaInicial",
                table: "ConsultaInicial",
                column: "ConsultaInicialId");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "HabitoAlimenticio",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "ConsultaInicialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paciente_ConsultaInicial_ConsultaInicialId",
                table: "Paciente",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "ConsultaInicialId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
