using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WladcyKostek.Repo.Migrations
{
    /// <inheritdoc />
    public partial class addBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Base64Img",
                table: "ScrappedNews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64Img",
                table: "ScrappedNews");
        }
    }
}
