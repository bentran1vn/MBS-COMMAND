using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS_COMMAND.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phonenumber",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Users",
                newName: "IsFirstLogin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFirstLogin",
                table: "Users",
                newName: "Gender");

            migrationBuilder.AddColumn<string>(
                name: "Phonenumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
