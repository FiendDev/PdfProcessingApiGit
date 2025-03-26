using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfProcessingApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSIAndNOFromPdfExtracData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NO",
                table: "PdfExtracData");

            migrationBuilder.DropColumn(
                name: "SI",
                table: "PdfExtracData");

            migrationBuilder.RenameColumn(
                name: "rutEmpresa",
                table: "PdfExtracData",
                newName: "RutEmpresa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RutEmpresa",
                table: "PdfExtracData",
                newName: "rutEmpresa");

            migrationBuilder.AddColumn<bool>(
                name: "NO",
                table: "PdfExtracData",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SI",
                table: "PdfExtracData",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
