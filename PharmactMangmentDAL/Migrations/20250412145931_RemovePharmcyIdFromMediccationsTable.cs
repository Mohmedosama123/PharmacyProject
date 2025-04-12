using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmactMangmentDAL.Migrations
{
    /// <inheritdoc />
    public partial class RemovePharmcyIdFromMediccationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Pharmacies_PharmacyId",
                table: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Medications_PharmacyId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Medications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PharmacyId",
                table: "Medications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medications_PharmacyId",
                table: "Medications",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Pharmacies_PharmacyId",
                table: "Medications",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id");
        }
    }
}
