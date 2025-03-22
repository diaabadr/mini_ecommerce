using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingProductCreator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "products",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_CreatorId",
                table: "products",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_AspNetUsers_CreatorId",
                table: "products",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_AspNetUsers_CreatorId",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_CreatorId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "products");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "products");
        }
    }
}
