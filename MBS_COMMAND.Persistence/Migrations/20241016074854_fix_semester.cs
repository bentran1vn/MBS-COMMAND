using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS_COMMAND.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_semester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Semesters",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Semesters");
        }
    }
}
