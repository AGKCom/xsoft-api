using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xsoft.Migrations
{
    /// <inheritdoc />
    public partial class updateConfigurationsDBCreds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user",
                table: "Configurations",
                newName: "dbUser");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Configurations",
                newName: "dbPassword");

            migrationBuilder.AddColumn<string>(
                name: "dbHost",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dbHost",
                table: "Configurations");

            migrationBuilder.RenameColumn(
                name: "dbUser",
                table: "Configurations",
                newName: "user");

            migrationBuilder.RenameColumn(
                name: "dbPassword",
                table: "Configurations",
                newName: "password");
        }
    }
}
