using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Data.Migrations
{
    public partial class CreateNamesColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Visits",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Visits",
                nullable: true);
        }

        
    }
}
