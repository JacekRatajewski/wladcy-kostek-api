using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WladcyKostek.Repo.Migrations
{
    /// <inheritdoc />
    public partial class addBonusesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bonuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BonusCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoneySupported = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonuses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bonuses");
        }
    }
}
