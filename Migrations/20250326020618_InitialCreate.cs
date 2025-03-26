using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfProcessingApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PdfExtracData",
                columns: table => new
                {
                    UID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    idFacultad = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    facultad = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SI = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    NO = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfExtracData", x => new { x.UID, x.idFacultad });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfExtracData");
        }
    }
}
