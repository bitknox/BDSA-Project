using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace QueueSafe.Api.Migrations
{
    public partial class create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Postal = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Postal);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    StreetName = table.Column<string>(type: "text", nullable: false),
                    CityPostal = table.Column<int>(type: "integer", nullable: false),
                    HouseNumber = table.Column<int>(type: "integer", nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => new { x.StreetName, x.HouseNumber, x.CityPostal });
                    table.ForeignKey(
                        name: "FK_Address_City_CityPostal",
                        column: x => x.CityPostal,
                        principalTable: "City",
                        principalColumn: "Postal",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Address_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Token = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    StoreId = table.Column<int>(type: "integer", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Token);
                    table.ForeignKey(
                        name: "FK_Booking_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CityPostal",
                table: "Address",
                column: "CityPostal");

            migrationBuilder.CreateIndex(
                name: "IX_Address_StoreId",
                table: "Address",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StoreId",
                table: "Booking",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Store");
        }
    }
}
