using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmactMangmentDAL.Migrations
{
    /// <inheritdoc />
    public partial class secafteraddImagename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Pharmacies");
        }
    }
}
