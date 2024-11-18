using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionPerfil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RazonSocialPension",
                table: "Pension",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RazonSocialPension",
                table: "Pension");
        }
    }
}
