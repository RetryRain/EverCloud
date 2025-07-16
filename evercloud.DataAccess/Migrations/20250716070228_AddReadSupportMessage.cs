using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace evercloud.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddReadSupportMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "SupportMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "SupportMessages");
        }
    }
}
