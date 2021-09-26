using Microsoft.EntityFrameworkCore.Migrations;

namespace IpharmWebAppProject.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Orders_OrderID",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Products_ProductID",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInWishLists_Products_ProductID",
                table: "ProductInWishLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInWishLists_WishLists_WishListID",
                table: "ProductInWishLists");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ReviewID",
                table: "Reviews",
                newName: "ReviewId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductInWishListID",
                table: "ProductInWishLists",
                newName: "ProductInWishListId");

            migrationBuilder.RenameColumn(
                name: "WishListID",
                table: "ProductInWishLists",
                newName: "WishListId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductInWishLists",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInWishLists_WishListID",
                table: "ProductInWishLists",
                newName: "IX_ProductInWishLists_WishListId");

            migrationBuilder.RenameColumn(
                name: "ProductInOrderID",
                table: "ProductInOrders",
                newName: "ProductInOrderId");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "ProductInOrders",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "ProductInOrders",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInOrders_OrderID",
                table: "ProductInOrders",
                newName: "IX_ProductInOrders_OrderId");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "Orders",
                newName: "OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PicUrl1",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserEmail",
                table: "Reviews",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Orders_OrderId",
                table: "ProductInOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Products_ProductId",
                table: "ProductInOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInWishLists_Products_ProductId",
                table: "ProductInWishLists",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInWishLists_WishLists_WishListId",
                table: "ProductInWishLists",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserEmail",
                table: "Reviews",
                column: "UserEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Orders_OrderId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInOrders_Products_ProductId",
                table: "ProductInOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInWishLists_Products_ProductId",
                table: "ProductInWishLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInWishLists_WishLists_WishListId",
                table: "ProductInWishLists");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserEmail",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserEmail",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Reviews",
                newName: "ReviewID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "ProductInWishListId",
                table: "ProductInWishLists",
                newName: "ProductInWishListID");

            migrationBuilder.RenameColumn(
                name: "WishListId",
                table: "ProductInWishLists",
                newName: "WishListID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductInWishLists",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInWishLists_WishListId",
                table: "ProductInWishLists",
                newName: "IX_ProductInWishLists_WishListID");

            migrationBuilder.RenameColumn(
                name: "ProductInOrderId",
                table: "ProductInOrders",
                newName: "ProductInOrderID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ProductInOrders",
                newName: "OrderID");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductInOrders",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_ProductInOrders_OrderId",
                table: "ProductInOrders",
                newName: "IX_ProductInOrders_OrderID");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Orders",
                newName: "OrderID");

            migrationBuilder.AlterColumn<string>(
                name: "Adress",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PicUrl1",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Orders_OrderID",
                table: "ProductInOrders",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInOrders_Products_ProductID",
                table: "ProductInOrders",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInWishLists_Products_ProductID",
                table: "ProductInWishLists",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInWishLists_WishLists_WishListID",
                table: "ProductInWishLists",
                column: "WishListID",
                principalTable: "WishLists",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
