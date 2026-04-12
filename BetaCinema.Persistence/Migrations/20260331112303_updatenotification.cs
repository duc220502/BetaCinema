using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetaCinema.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updatenotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGlobal",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "AudienceType",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DispatchStatus",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudienceType",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "DispatchStatus",
                table: "Notifications");

            migrationBuilder.AddColumn<bool>(
                name: "IsGlobal",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
