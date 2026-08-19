using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PersonaStatsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialStats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SocialStats",
                columns: new[] { "Id", "Level", "Name", "Points" },
                values: new object[,]
                {
                    { 1, 1, "Conocimiento", 0 },
                    { 2, 1, "Coraje", 0 },
                    { 3, 1, "Amabilidad", 0 },
                    { 4, 1, "Proeza", 0 },
                    { 5, 1, "Valentia", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialStats");
        }
    }
}
