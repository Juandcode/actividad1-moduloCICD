using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "HabitoAlimenticio");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConsultaInicialId",
                table: "HabitoAlimenticio",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "HabitoAlimenticio",
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

            migrationBuilder.AlterColumn<Guid>(
                name: "ConsultaInicialId",
                table: "HabitoAlimenticio",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                table: "HabitoAlimenticio",
                column: "ConsultaInicialId",
                principalTable: "ConsultaInicial",
                principalColumn: "Id");
        }
    }
}
