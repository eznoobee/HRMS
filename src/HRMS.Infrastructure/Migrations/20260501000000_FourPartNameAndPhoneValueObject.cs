using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FourPartNameAndPhoneValueObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename LastName → FamilyName
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Employees",
                newName: "FamilyName");

            // Add FatherName (NOT NULL, default '' for existing rows)
            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "Employees",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // Add GrandfatherName (NOT NULL, default '' for existing rows)
            migrationBuilder.AddColumn<string>(
                name: "GrandfatherName",
                table: "Employees",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            // Phone column stays varchar(20) — value converter handles normalization in app layer,
            // no schema change needed.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "FatherName", table: "Employees");
            migrationBuilder.DropColumn(name: "GrandfatherName", table: "Employees");

            migrationBuilder.RenameColumn(
                name: "FamilyName",
                table: "Employees",
                newName: "LastName");
        }
    }
}
