using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmactMangmentDAL.Migrations
{
    /// <inheritdoc />
    public partial class addmedPharsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Med_Phar_Medications_MedicationId",
                table: "Med_Phar");

            migrationBuilder.DropForeignKey(
                name: "FK_Med_Phar_Pharmacies_PharmacyId",
                table: "Med_Phar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Med_Phar",
                table: "Med_Phar");

            migrationBuilder.RenameTable(
                name: "Med_Phar",
                newName: "Med_Phars");

            migrationBuilder.RenameIndex(
                name: "IX_Med_Phar_PharmacyId",
                table: "Med_Phars",
                newName: "IX_Med_Phars_PharmacyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Med_Phars",
                table: "Med_Phars",
                columns: new[] { "MedicationId", "PharmacyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Med_Phars_Medications_MedicationId",
                table: "Med_Phars",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Med_Phars_Pharmacies_PharmacyId",
                table: "Med_Phars",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Med_Phars_Medications_MedicationId",
                table: "Med_Phars");

            migrationBuilder.DropForeignKey(
                name: "FK_Med_Phars_Pharmacies_PharmacyId",
                table: "Med_Phars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Med_Phars",
                table: "Med_Phars");

            migrationBuilder.RenameTable(
                name: "Med_Phars",
                newName: "Med_Phar");

            migrationBuilder.RenameIndex(
                name: "IX_Med_Phars_PharmacyId",
                table: "Med_Phar",
                newName: "IX_Med_Phar_PharmacyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Med_Phar",
                table: "Med_Phar",
                columns: new[] { "MedicationId", "PharmacyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Med_Phar_Medications_MedicationId",
                table: "Med_Phar",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Med_Phar_Pharmacies_PharmacyId",
                table: "Med_Phar",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
