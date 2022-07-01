using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace StarboardNFT.Data.Migrations
{
    public partial class AddCollectionIdToNFTTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                 name: "CollectionId",
                 table: "NFT",
                 type: "uniqueidentifier",
                 nullable: true,
                 defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "NFT"
            );
        }
    }
}
