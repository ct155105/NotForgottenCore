using Microsoft.EntityFrameworkCore.Migrations;

namespace NotForgottenCore.Migrations
{
    public partial class UpdatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OpenSeats",
                table: "Tables",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GroupMembers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "GroupMembers");

            migrationBuilder.AlterColumn<int>(
                name: "OpenSeats",
                table: "Tables",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
