using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluacion",
                table: "Evaluacion");

            migrationBuilder.DropColumn(
                name: "EvaluacionId",
                table: "Evaluacion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluacion",
                table: "Evaluacion",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Evaluacion",
                table: "Evaluacion");

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluacionId",
                table: "Evaluacion",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Evaluacion",
                table: "Evaluacion",
                column: "EvaluacionId");
        }
    }
}
