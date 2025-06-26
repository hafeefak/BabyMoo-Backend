using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabyMoo.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Products_ProductId",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Users_UserId",
                table: "Wishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist");

            migrationBuilder.RenameTable(
                name: "Wishlist",
                newName: "Wishlists");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_UserId_ProductId",
                table: "Wishlists",
                newName: "IX_Wishlists_UserId_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_ProductId",
                table: "Wishlists",
                newName: "IX_Wishlists_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Products_ProductId",
                table: "Wishlists",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Users_UserId",
                table: "Wishlists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Products_ProductId",
                table: "Wishlists");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Users_UserId",
                table: "Wishlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlists",
                table: "Wishlists");

            migrationBuilder.RenameTable(
                name: "Wishlists",
                newName: "Wishlist");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_UserId_ProductId",
                table: "Wishlist",
                newName: "IX_Wishlist_UserId_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlists_ProductId",
                table: "Wishlist",
                newName: "IX_Wishlist_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Products_ProductId",
                table: "Wishlist",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Users_UserId",
                table: "Wishlist",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
