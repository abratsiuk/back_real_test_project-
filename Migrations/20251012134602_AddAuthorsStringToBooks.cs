using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_test_project.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorsStringToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorsString",
                table: "Books",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorsString",
                table: "Books");
        }
    }
}
