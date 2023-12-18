using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetCoreIdentity.Migrations
{
    public partial class updateAppUserBirthDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "AspNetUsers",
                newName: "BirthDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "AspNetUsers",
                newName: "Birthday");
        }
    }
}
