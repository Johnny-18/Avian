using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Avian.Dal.Migrations
{
    /// <inheritdoc />
    public partial class ChangeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "Types",
                table: "tickets",
                newName: "Type");

            migrationBuilder.AlterColumn<Guid>(
                name: "PlaneId",
                table: "flights",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid[]>(
                name: "Pilots",
                table: "flights",
                type: "uuid[]",
                nullable: true,
                oldClrType: typeof(Guid[]),
                oldType: "uuid[]");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "tickets",
                newName: "Types");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "PlaneId",
                table: "flights",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid[]>(
                name: "Pilots",
                table: "flights",
                type: "uuid[]",
                nullable: false,
                defaultValue: new Guid[0],
                oldClrType: typeof(Guid[]),
                oldType: "uuid[]",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");
        }
    }
}
