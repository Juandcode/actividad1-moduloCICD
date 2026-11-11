using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionClinicaNutricional.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixOutboxModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages");

            migrationBuilder.EnsureSchema(
                name: "outbox");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                newName: "outboxMessage",
                newSchema: "outbox");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "outbox",
                table: "outboxMessage",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "TraceId",
                schema: "outbox",
                table: "outboxMessage",
                newName: "traceId");

            migrationBuilder.RenameColumn(
                name: "SpanId",
                schema: "outbox",
                table: "outboxMessage",
                newName: "spanId");

            migrationBuilder.RenameColumn(
                name: "ProcessedOn",
                schema: "outbox",
                table: "outboxMessage",
                newName: "processedOn");

            migrationBuilder.RenameColumn(
                name: "Processed",
                schema: "outbox",
                table: "outboxMessage",
                newName: "processed");

            migrationBuilder.RenameColumn(
                name: "Created",
                schema: "outbox",
                table: "outboxMessage",
                newName: "created");

            migrationBuilder.RenameColumn(
                name: "CorrelationId",
                schema: "outbox",
                table: "outboxMessage",
                newName: "correlationId");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "outbox",
                table: "outboxMessage",
                newName: "outboxId");

            migrationBuilder.AddColumn<string>(
                name: "content",
                schema: "outbox",
                table: "outboxMessage",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_outboxMessage",
                schema: "outbox",
                table: "outboxMessage",
                column: "outboxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_outboxMessage",
                schema: "outbox",
                table: "outboxMessage");

            migrationBuilder.DropColumn(
                name: "content",
                schema: "outbox",
                table: "outboxMessage");

            migrationBuilder.RenameTable(
                name: "outboxMessage",
                schema: "outbox",
                newName: "OutboxMessages");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "OutboxMessages",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "traceId",
                table: "OutboxMessages",
                newName: "TraceId");

            migrationBuilder.RenameColumn(
                name: "spanId",
                table: "OutboxMessages",
                newName: "SpanId");

            migrationBuilder.RenameColumn(
                name: "processedOn",
                table: "OutboxMessages",
                newName: "ProcessedOn");

            migrationBuilder.RenameColumn(
                name: "processed",
                table: "OutboxMessages",
                newName: "Processed");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "OutboxMessages",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "correlationId",
                table: "OutboxMessages",
                newName: "CorrelationId");

            migrationBuilder.RenameColumn(
                name: "outboxId",
                table: "OutboxMessages",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                table: "OutboxMessages",
                column: "Id");
        }
    }
}
