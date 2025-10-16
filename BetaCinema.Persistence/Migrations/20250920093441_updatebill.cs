using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetaCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatebill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Bills");

            migrationBuilder.InsertData(
                table: "BillStatuses",
                columns: new[] { "Id", "Code", "StatusName" },
                values: new object[] { 7, "ReconciliationFailed", "Đối soát thất bại" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BillStatuses",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Bills",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
