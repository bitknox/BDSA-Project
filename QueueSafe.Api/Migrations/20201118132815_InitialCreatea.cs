﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QueueSafe.Api.Migrations
{
    public partial class InitialCreatea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Store",
                columns: new[] { "Id", "Capacity", "Name" },
                values: new object[] { 1, 50, "ElGigadik" });

            migrationBuilder.InsertData(
                table: "Booking",
                columns: new[] { "Token", "State", "StoreId", "TimeStamp" },
                values: new object[] { "hbkHBAKBKHSDS/", 0, 1, new DateTime(2020, 11, 18, 14, 28, 15, 504, DateTimeKind.Local).AddTicks(9227) });

            migrationBuilder.InsertData(
                table: "Booking",
                columns: new[] { "Token", "State", "StoreId", "TimeStamp" },
                values: new object[] { "hbkHBasdAKBKHSDS/", 0, 1, new DateTime(2020, 11, 18, 14, 28, 15, 508, DateTimeKind.Local).AddTicks(1361) });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_StoreId",
                table: "Booking",
                column: "StoreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Store");
        }
    }
}