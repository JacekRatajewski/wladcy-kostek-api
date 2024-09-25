using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WładcyKostek.Repo.Migrations
{
    /// <inheritdoc />
    public partial class add_sent_flag_to_news : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "News",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sent",
                table: "News");
        }
    }
}
