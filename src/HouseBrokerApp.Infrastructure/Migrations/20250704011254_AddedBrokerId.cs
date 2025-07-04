using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseBrokerApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedBrokerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyListings_AspNetUsers_ApplicationUserId",
                table: "PropertyListings");

            migrationBuilder.DropIndex(
                name: "IX_PropertyListings_ApplicationUserId",
                table: "PropertyListings");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "PropertyListings");

            migrationBuilder.AddColumn<int>(
                name: "BrokerId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_BrokerId",
                table: "AspNetUsers",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_BrokerId",
                table: "PropertyListings",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BrokerId",
                table: "AspNetUsers",
                column: "BrokerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyListings_AspNetUsers_BrokerId",
                table: "PropertyListings",
                column: "BrokerId",
                principalTable: "AspNetUsers",
                principalColumn: "BrokerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyListings_AspNetUsers_BrokerId",
                table: "PropertyListings");

            migrationBuilder.DropIndex(
                name: "IX_PropertyListings_BrokerId",
                table: "PropertyListings");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_BrokerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BrokerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "PropertyListings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyListings_ApplicationUserId",
                table: "PropertyListings",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyListings_AspNetUsers_ApplicationUserId",
                table: "PropertyListings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
