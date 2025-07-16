using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace evercloud.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageContentToSupportMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "SupportMessages",
                newName: "MessageContent");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SupportMessages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_UserId",
                table: "SupportMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupportMessages_AspNetUsers_UserId",
                table: "SupportMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportMessages_AspNetUsers_UserId",
                table: "SupportMessages");

            migrationBuilder.DropIndex(
                name: "IX_SupportMessages_UserId",
                table: "SupportMessages");

            migrationBuilder.RenameColumn(
                name: "MessageContent",
                table: "SupportMessages",
                newName: "Content");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SupportMessages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
