using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStream.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyJobDetail_RemoveColumn_IsCancelled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_cancellation_requested",
                table: "job_details");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_cancellation_requested",
                table: "job_details",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
