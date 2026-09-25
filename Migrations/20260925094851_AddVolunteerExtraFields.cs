using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gift_of_the_Givers_Relief_App.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteerExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Volunteers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DisasterID",
                table: "Volunteers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasOwnTransport",
                table: "Volunteers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Volunteers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "DisasterID",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "HasOwnTransport",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Volunteers");
        }
    }
}
