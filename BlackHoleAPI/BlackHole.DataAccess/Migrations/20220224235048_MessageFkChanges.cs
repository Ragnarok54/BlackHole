using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackHole.DataAccess.Migrations
{
    public partial class MessageFkChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblMessage_tblConversation_MessageId",
                table: "tblMessage");

            migrationBuilder.AddColumn<Guid>(
                name: "LastConversationConversationId",
                table: "tblMessage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblMessage_LastConversationConversationId",
                table: "tblMessage",
                column: "LastConversationConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_tblConversation_LastMessageId",
                table: "tblConversation",
                column: "LastMessageId",
                unique: true,
                filter: "[LastMessageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_tblConversation_tblMessage_LastMessageId",
                table: "tblConversation",
                column: "LastMessageId",
                principalTable: "tblMessage",
                principalColumn: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblMessage_tblConversation_LastConversationConversationId",
                table: "tblMessage",
                column: "LastConversationConversationId",
                principalTable: "tblConversation",
                principalColumn: "ConversationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblConversation_tblMessage_LastMessageId",
                table: "tblConversation");

            migrationBuilder.DropForeignKey(
                name: "FK_tblMessage_tblConversation_LastConversationConversationId",
                table: "tblMessage");

            migrationBuilder.DropIndex(
                name: "IX_tblMessage_LastConversationConversationId",
                table: "tblMessage");

            migrationBuilder.DropIndex(
                name: "IX_tblConversation_LastMessageId",
                table: "tblConversation");

            migrationBuilder.DropColumn(
                name: "LastConversationConversationId",
                table: "tblMessage");

            migrationBuilder.AddForeignKey(
                name: "FK_tblMessage_tblConversation_MessageId",
                table: "tblMessage",
                column: "MessageId",
                principalTable: "tblConversation",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
