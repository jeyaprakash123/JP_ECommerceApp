using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceApp.Product.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "catagories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatagoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catagories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "productss",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ManufactureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productss", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_productss_catagories_Id",
                        column: x => x.Id,
                        principalTable: "catagories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "catagories",
                columns: new[] { "Id", "CatagoryName" },
                values: new object[,]
                {
                    { 1, "Fruits" },
                    { 2, "Vegetables" }
                });

            migrationBuilder.InsertData(
                table: "productss",
                columns: new[] { "ProductId", "ExpiryDate", "Id", "ManufactureDate", "Price", "ProductName" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 5, 21), 1, new DateOnly(2025, 2, 21), 150.0, "Apple" },
                    { 2, new DateOnly(2025, 2, 24), 2, new DateOnly(2025, 2, 19), 42.0, "Brinjal" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_productss_Id",
                table: "productss",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productss");

            migrationBuilder.DropTable(
                name: "catagories");
        }
    }
}
