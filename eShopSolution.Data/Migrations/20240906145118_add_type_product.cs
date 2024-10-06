using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class add_type_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderFeatured",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Purpose",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "f2294168-c969-47e3-98f0-3503d3b88f0c");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "6d9bee0b-e443-4732-8d0b-dac1734f25c8", "AQAAAAEAACcQAAAAEMwOTKCN9gG1R7gWVct5wX8lJICEvXS9+ke5cfN/JGWnOXyam/IptRKP2Q0wDzyz4g==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "OrderFeatured", "Purpose" },
                values: new object[] { new DateTime(2024, 9, 6, 21, 51, 17, 663, DateTimeKind.Local).AddTicks(5332), 5, 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "OrderFeatured", "Purpose" },
                values: new object[] { new DateTime(2024, 9, 6, 21, 51, 17, 664, DateTimeKind.Local).AddTicks(2293), 5, 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "OrderFeatured", "Purpose" },
                values: new object[] { new DateTime(2024, 9, 6, 21, 51, 17, 664, DateTimeKind.Local).AddTicks(2324), 5, 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "OrderFeatured", "Purpose" },
                values: new object[] { new DateTime(2024, 9, 6, 21, 51, 17, 664, DateTimeKind.Local).AddTicks(2327), 5, 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "OrderFeatured", "Purpose" },
                values: new object[] { new DateTime(2024, 9, 6, 21, 51, 17, 664, DateTimeKind.Local).AddTicks(2328), 5, 5 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "OrderFeatured", "Purpose" },
                values: new object[] { new DateTime(2024, 9, 6, 21, 51, 17, 664, DateTimeKind.Local).AddTicks(2329), 5, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderFeatured",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"),
                column: "ConcurrencyStamp",
                value: "9a284855-7045-46ea-bd98-87986ef9fc47");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "23649586-f25b-457e-b2ef-7fad4dd2c4f1", "AQAAAAEAACcQAAAAEO4KvFINkiETS0SmuG7A610n54FsYLyp7uJQspTv7Y6KlbIl75yGGmGcOcaJYuZV4A==" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 9, 3, 21, 44, 18, 958, DateTimeKind.Local).AddTicks(3845));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2024, 9, 3, 21, 44, 18, 959, DateTimeKind.Local).AddTicks(787));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2024, 9, 3, 21, 44, 18, 959, DateTimeKind.Local).AddTicks(822));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2024, 9, 3, 21, 44, 18, 959, DateTimeKind.Local).AddTicks(825));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2024, 9, 3, 21, 44, 18, 959, DateTimeKind.Local).AddTicks(826));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2024, 9, 3, 21, 44, 18, 959, DateTimeKind.Local).AddTicks(827));
        }
    }
}
