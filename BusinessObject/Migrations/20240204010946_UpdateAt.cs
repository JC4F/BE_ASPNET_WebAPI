using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    public partial class UpdateAt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Whislists",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "User",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "ProductVariants",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Product",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Payment",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "OrderItem",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Order",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Comment",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Category",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "BlogRating",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "Blog",
                newName: "UpdatedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Whislists",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "User",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "ProductVariants",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Product",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Payment",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "OrderItem",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Order",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Comment",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Category",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "BlogRating",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Blog",
                newName: "UpdatedBy");
        }
    }
}
