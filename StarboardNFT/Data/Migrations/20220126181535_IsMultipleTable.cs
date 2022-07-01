using Microsoft.EntityFrameworkCore.Migrations;

namespace StarboardNFT.Data.Migrations
{
    public partial class IsMultipleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMultiple",
                table: "NFT",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMultiple",
                table: "NFT");
        }
    }
}
