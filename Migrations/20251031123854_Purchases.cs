using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealer.Migrations
{
    /// <inheritdoc />
    public partial class Purchases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Clients_clientId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_clientId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "clientId",
                table: "Vehicles");

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    client = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.id);
                    table.ForeignKey(
                        name: "FK_Purchases_Clients_client",
                        column: x => x.client,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_Vehicles_vehicle",
                        column: x => x.vehicle,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_client",
                table: "Purchases",
                column: "client");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_vehicle",
                table: "Purchases",
                column: "vehicle",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.AddColumn<Guid>(
                name: "clientId",
                table: "Vehicles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_clientId",
                table: "Vehicles",
                column: "clientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Clients_clientId",
                table: "Vehicles",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
