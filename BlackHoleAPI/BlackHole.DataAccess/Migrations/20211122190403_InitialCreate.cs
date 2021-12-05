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
                    UserId = table.Column<Guid>(nullable: false),
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
                name: "tblConversation",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LastMessageId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblConversation", x => x.ConversationId);
                    table.ForeignKey(
                        name: "FK_tblConversation_tblUser_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblMessage",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    ConversationId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    SenderUserId = table.Column<Guid>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    Seen = table.Column<bool>(nullable: false)
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
                        name: "FK_tblMessage_tblConversation_MessageId",
                        column: x => x.MessageId,
                        principalTable: "tblConversation",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblMessage_tblUser_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "tblUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblUserConversation",
                columns: table => new
                {
                    UserConversationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ConversationId = table.Column<Guid>(nullable: false),
                    UserId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserConversation", x => x.UserConversationId);
                    table.ForeignKey(
                        name: "FK_tblUserConversation_tblConversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "tblConversation",
                        principalColumn: "ConversationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblUserConversation_tblUser_UserId1",
                        column: x => x.UserId1,
                        principalTable: "tblUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblAttachment_AttachmentTypeId",
                table: "tblAttachment",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblConversation_UserId",
                table: "tblConversation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMessage_AttachmentId",
                table: "tblMessage",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMessage_SenderUserId",
                table: "tblMessage",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserConversation_ConversationId",
                table: "tblUserConversation",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserConversation_UserId1",
                table: "tblUserConversation",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblMessage");

            migrationBuilder.DropTable(
                name: "tblUserConversation");

            migrationBuilder.DropTable(
                name: "tblAttachment");

            migrationBuilder.DropTable(
                name: "tblConversation");

            migrationBuilder.DropTable(
                name: "tblAttachmentType");

            migrationBuilder.DropTable(
                name: "tblUser");
        }
    }
}
