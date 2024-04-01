using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace xsoft.Migrations
{
    /// <inheritdoc />
    public partial class updateConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "companyName",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "database",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "user",
                table: "Configurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "companyName",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "database",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "password",
                table: "Configurations");

            migrationBuilder.DropColumn(
                name: "user",
                table: "Configurations");
        }
    }
}
