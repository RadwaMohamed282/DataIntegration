using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Integration.Migrations
{
    public partial class AddRewardLoyaltyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RewardLoyaltys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointsValue = table.Column<int>(type: "int", nullable: false),
                    ExternalLogId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardLoyaltys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardLoyaltys");
        }
    }
}
