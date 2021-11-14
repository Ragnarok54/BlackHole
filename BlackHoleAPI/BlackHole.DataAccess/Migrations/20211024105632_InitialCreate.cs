using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackHole.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblAttachmentType",
                columns: table => new
                {
                    AttachmentTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAttachmentType", x => x.AttachmentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "tblUser",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    Picture = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUser", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tblAttachment",
                columns: table => new
                {
                    AttachmentId = table.Column<Guid>(nullable: false),
                    MessageId = table.Column<long>(nullable: false),
                    Obejct = table.Column<byte[]>(nullable: false),
                    AttachamentTypeId = table.Column<int>(nullable: false),
                    AttachmentTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblAttachment", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_tblAttachment_tblAttachmentType_AttachmentTypeId",
                        column: x => x.AttachmentTypeId,
                        principalTable: "tblAttachmentType",
                        principalColumn: "AttachmentTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblMessage",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    FromUserId = table.Column<int>(nullable: false),
                    ToUserId = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    AttachmentId = table.Column<Guid>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),
                    SeenOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMessage", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_tblMessage_tblAttachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "tblAttachment",
                        principalColumn: "AttachmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblMessage_tblUser_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "tblUser",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_tblMessage_tblUser_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "tblUser",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAttachment_AttachmentTypeId",
                table: "tblAttachment",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMessage_AttachmentId",
                table: "tblMessage",
                column: "AttachmentId",
                unique: true,
                filter: "[AttachmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblMessage_FromUserId",
                table: "tblMessage",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMessage_ToUserId",
                table: "tblMessage",
                column: "ToUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblMessage");

            migrationBuilder.DropTable(
                name: "tblAttachment");

            migrationBuilder.DropTable(
                name: "tblUser");

            migrationBuilder.DropTable(
                name: "tblAttachmentType");
        }
    }
}
