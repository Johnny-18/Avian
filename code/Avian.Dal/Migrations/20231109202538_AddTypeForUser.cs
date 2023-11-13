using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avian.Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "tickets",
                newName: "Types");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "Types",
                table: "tickets",
                newName: "Type");
        }
    }
}
