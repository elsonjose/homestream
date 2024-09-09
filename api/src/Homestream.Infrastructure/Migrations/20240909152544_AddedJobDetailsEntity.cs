using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HomeStream.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedJobDetailsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_details",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    job_type_name = table.Column<string>(type: "text", nullable: false),
                    job_payload_type_name = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "text", nullable: false),
                    is_cancellation_requested = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    progress = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    exception_message = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    stack_trace = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    created_on = table.Column<long>(type: "bigint", nullable: false),
                    started_on = table.Column<long>(type: "bigint", nullable: true),
                    completed_on = table.Column<long>(type: "bigint", nullable: true),
                    cancellation_request_on = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_job_details", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_details");
        }
    }
}
