using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsultaInicial",
                columns: table => new
                {
                    ConsultaInicialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Peso = table.Column<int>(type: "int", nullable: false),
                    Altura = table.Column<double>(type: "float", nullable: false),
                    Composicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Antecedentes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsultaInicial", x => x.ConsultaInicialId);
                });

            migrationBuilder.CreateTable(
                name: "PlanAlimenticio",
                columns: table => new
                {
                    PlanAlimenticioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuracionPlan = table.Column<int>(type: "int", nullable: false),
                    EstadoPlan = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanAlimenticio", x => x.PlanAlimenticioId);
                });

            migrationBuilder.CreateTable(
                name: "HabitoAlimenticio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoComida_Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoComida_Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoComida_Categoria = table.Column<int>(type: "int", nullable: false),
                    ConsultaInicialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitoAlimenticio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HabitoAlimenticio_ConsultaInicial_ConsultaInicialId",
                        column: x => x.ConsultaInicialId,
                        principalTable: "ConsultaInicial",
                        principalColumn: "ConsultaInicialId");
                });

            migrationBuilder.CreateTable(
                name: "Evaluacion",
                columns: table => new
                {
                    EvaluacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlanAlimenticioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluacion", x => x.EvaluacionId);
                    table.ForeignKey(
                        name: "FK_Evaluacion_PlanAlimenticio_PlanAlimenticioId",
                        column: x => x.PlanAlimenticioId,
                        principalTable: "PlanAlimenticio",
                        principalColumn: "PlanAlimenticioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultaInicialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanAlimenticioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.PacienteId);
                    table.ForeignKey(
                        name: "FK_Paciente_ConsultaInicial_ConsultaInicialId",
                        column: x => x.ConsultaInicialId,
                        principalTable: "ConsultaInicial",
                        principalColumn: "ConsultaInicialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Paciente_PlanAlimenticio_PlanAlimenticioId",
                        column: x => x.PlanAlimenticioId,
                        principalTable: "PlanAlimenticio",
                        principalColumn: "PlanAlimenticioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evaluacion_PlanAlimenticioId",
                table: "Evaluacion",
                column: "PlanAlimenticioId");

            migrationBuilder.CreateIndex(
                name: "IX_HabitoAlimenticio_ConsultaInicialId",
                table: "HabitoAlimenticio",
                column: "ConsultaInicialId");

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_ConsultaInicialId",
                table: "Paciente",
                column: "ConsultaInicialId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paciente_PlanAlimenticioId",
                table: "Paciente",
                column: "PlanAlimenticioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Evaluacion");

            migrationBuilder.DropTable(
                name: "HabitoAlimenticio");

            migrationBuilder.DropTable(
                name: "Paciente");

            migrationBuilder.DropTable(
                name: "ConsultaInicial");

            migrationBuilder.DropTable(
                name: "PlanAlimenticio");
        }
    }
}
