using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfProcessingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRutEmpresaToPdfExtracData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rutEmpresa",
                table: "PdfExtracData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rutEmpresa",
                table: "PdfExtracData");
        }
    }
}
