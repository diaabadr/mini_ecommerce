using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addCreatorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_products_AspNetUsers_CreatorId",
                table: "products",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
