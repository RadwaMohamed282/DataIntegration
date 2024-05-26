using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Integration.Migrations
{
    public partial class NewModelFeature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "SubscribeToOffers",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "RewardLoyaltys",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "SubscribeToOffers");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "RewardLoyaltys");
        }
    }
}
