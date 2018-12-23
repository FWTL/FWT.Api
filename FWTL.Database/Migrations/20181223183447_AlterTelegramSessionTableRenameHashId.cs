using Microsoft.EntityFrameworkCore.Migrations;

namespace FWTL.Database.Migrations
{
    public partial class AlterTelegramSessionTableRenameHashId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HashId",
                table: "TelegramSession",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TelegramSession",
                newName: "HashId");
        }
    }
}
