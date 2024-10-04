using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WladcyKostek.Repo.Migrations
{
    /// <inheritdoc />
    public partial class add_video_url_to_news : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "News",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "News");
        }
    }
}
