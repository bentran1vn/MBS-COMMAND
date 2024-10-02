using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS_COMMAND.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_entity_from_string_to_guid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Categories_CategoryId",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Certificates",
                newName: "SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_CategoryId",
                table: "Certificates",
                newName: "IX_Certificates_SkillId");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Slots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Slots",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateId",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Schedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LeaderId",
                table: "Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LeaderId",
                table: "Groups",
                column: "LeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Skills_SkillId",
                table: "Certificates",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_LeaderId",
                table: "Groups",
                column: "LeaderId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Skills_SkillId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_LeaderId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_LeaderId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Slots");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "LeaderId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "Certificates",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Certificates_SkillId",
                table: "Certificates",
                newName: "IX_Certificates_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Categories_CategoryId",
                table: "Certificates",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
