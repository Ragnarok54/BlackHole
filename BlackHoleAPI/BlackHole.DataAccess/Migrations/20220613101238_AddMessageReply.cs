using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackHole.DataAccess.Migrations
{
    public partial class AddMessageReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblMessage_tblConversation_LastConversationConversationId",
                table: "tblMessage");

            migrationBuilder.RenameColumn(
                name: "LastConversationConversationId",
                table: "tblMessage",
                newName: "RepliedMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_tblMessage_LastConversationConversationId",
                table: "tblMessage",
                newName: "IX_tblMessage_RepliedMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblMessage_tblMessage_RepliedMessageId",
                table: "tblMessage",
                column: "RepliedMessageId",
                principalTable: "tblMessage",
                principalColumn: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblMessage_tblMessage_RepliedMessageId",
                table: "tblMessage");

            migrationBuilder.RenameColumn(
                name: "RepliedMessageId",
                table: "tblMessage",
                newName: "LastConversationConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_tblMessage_RepliedMessageId",
                table: "tblMessage",
                newName: "IX_tblMessage_LastConversationConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblMessage_tblConversation_LastConversationConversationId",
                table: "tblMessage",
                column: "LastConversationConversationId",
                principalTable: "tblConversation",
                principalColumn: "ConversationId");
        }
    }
}
