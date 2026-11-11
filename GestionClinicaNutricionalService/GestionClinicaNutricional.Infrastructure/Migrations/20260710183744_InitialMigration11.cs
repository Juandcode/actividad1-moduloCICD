using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoComida_Descripcion",
                table: "HabitoAlimenticio");

            migrationBuilder.RenameColumn(
                name: "TipoComida_Nombre",
                table: "HabitoAlimenticio",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "TipoComida_Categoria",
                table: "HabitoAlimenticio",
                newName: "Categoria");

            migrationBuilder.CreateTable(
                name: "PlanComida",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanAlimenticioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanComida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanComida_PlanAlimenticio_PlanAlimenticioId",
                        column: x => x.PlanAlimenticioId,
                        principalTable: "PlanAlimenticio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanComida_PlanAlimenticioId",
                table: "PlanComida",
                column: "PlanAlimenticioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanComida");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "HabitoAlimenticio",
                newName: "TipoComida_Nombre");

            migrationBuilder.RenameColumn(
                name: "Categoria",
                table: "HabitoAlimenticio",
                newName: "TipoComida_Categoria");

            migrationBuilder.AddColumn<string>(
                name: "TipoComida_Descripcion",
                table: "HabitoAlimenticio",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
