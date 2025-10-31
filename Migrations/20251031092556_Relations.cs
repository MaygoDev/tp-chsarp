using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealer.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "clientId",
                table: "Vehicles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_clientId",
                table: "Vehicles",
                column: "clientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Clients_clientId",
                table: "Vehicles",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
