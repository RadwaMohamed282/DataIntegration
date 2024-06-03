using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Integration.Migrations
{
    public partial class updateTableFtp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CouponNumber",
                table: "SubscribeToOffersFTP",
                newName: "OfferNumbers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OfferNumbers",
                table: "SubscribeToOffersFTP",
                newName: "CouponNumber");
        }
    }
}
