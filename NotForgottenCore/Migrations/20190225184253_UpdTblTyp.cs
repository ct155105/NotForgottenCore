using Microsoft.EntityFrameworkCore.Migrations;

namespace NotForgottenCore.Migrations
{
    public partial class UpdTblTyp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TableType",
                table: "Tables",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TableType",
                table: "Tables");
        }
    }
}
