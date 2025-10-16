using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetaCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addexpiretime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpirationTimeInMinutes",
                table: "PaymentMethods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "Bills",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExpirationTimeInMinutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 2,
                column: "ExpirationTimeInMinutes",
                value: null);

            migrationBuilder.UpdateData(
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: 3,
                column: "ExpirationTimeInMinutes",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationTimeInMinutes",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "Bills");
        }
    }
}
