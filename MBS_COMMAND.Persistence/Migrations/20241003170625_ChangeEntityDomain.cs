using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MBS_COMMAND.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Skills_SkillId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Categories_CategoryId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_SkillId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CertificateId",
                table: "Skills");

            migrationBuilder.RenameColumn(
                name: "SkillId",
                table: "Certificates",
                newName: "MetorSkillsId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Skills",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MentorSkillsId",
                table: "Certificates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MentorSkillses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorSkillses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorSkillses_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorSkillses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_MentorSkillsId",
                table: "Certificates",
                column: "MentorSkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorSkillses_SkillId",
                table: "MentorSkillses",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorSkillses_UserId",
                table: "MentorSkillses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_MentorSkillses_MentorSkillsId",
                table: "Certificates",
                column: "MentorSkillsId",
                principalTable: "MentorSkillses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Categories_CategoryId",
                table: "Skills",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_MentorSkillses_MentorSkillsId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Categories_CategoryId",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "MentorSkillses");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_MentorSkillsId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "MentorSkillsId",
                table: "Certificates");

            migrationBuilder.RenameColumn(
                name: "MetorSkillsId",
                table: "Certificates",
                newName: "SkillId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Skills",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "CertificateId",
                table: "Skills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_SkillId",
                table: "Certificates",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Skills_SkillId",
                table: "Certificates",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Categories_CategoryId",
                table: "Skills",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
