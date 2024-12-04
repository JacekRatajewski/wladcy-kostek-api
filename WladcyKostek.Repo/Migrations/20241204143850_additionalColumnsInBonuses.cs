using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WladcyKostek.Repo.Migrations
{
    /// <inheritdoc />
    public partial class additionalColumnsInBonuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Bonuses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerSeasonStart",
                table: "Bonuses",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Bonuses");

            migrationBuilder.DropColumn(
                name: "PlayerSeasonStart",
                table: "Bonuses");
        }
    }
}
