using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClusteredIndexOnRaCpf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Students_RA_CPF",
                table: "Students",
                columns: new[] { "RA", "CPF" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_RA_CPF",
                table: "Students");
        }
    }
}
