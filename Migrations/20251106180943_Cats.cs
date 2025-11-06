using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_test_project.Migrations
{
    /// <inheritdoc />
    public partial class Cats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Cats",
                columns: new[] { "Id", "Age", "Name" },
                values: new object[,]
                {
                    { 1, 6, "Garfield" },
                    { 2, 5, "Tom" },
                    { 3, 7, "Sylvester" },
                    { 4, 8, "Cheshire" },
                    { 5, 4, "Simba" },
                    { 6, 3, "Nala" },
                    { 7, 5, "Puss in Boots" },
                    { 8, 9, "Felix" },
                    { 9, 10, "Salem" },
                    { 10, 2, "Snowball" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cats");
        }
    }
}
