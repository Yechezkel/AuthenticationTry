using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthenticationTry.Migrations
{
    /// <inheritdoc />
    public partial class Migrate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "User",
                type: "nvarchar(max)",
                nullable: true)
                ;
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "User");
        }
    }
}
