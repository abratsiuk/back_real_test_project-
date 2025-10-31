using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_test_project.Migrations
{
    /// <inheritdoc />
    public partial class AddBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Authors = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    PublicationYear = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Authors", "Description", "PublicationYear", "Title" },
                values: new object[,]
                {
                    { 1, "Herman Melville", null, 1851, "Moby-Dick" },
                    { 2, "Mark Twain", null, 1884, "The Adventures of Huckleberry Finn" },
                    { 3, "F. Scott Fitzgerald", null, 1925, "The Great Gatsby" },
                    { 4, "Harper Lee", null, 1960, "To Kill a Mockingbird" },
                    { 5, "J. D. Salinger", null, 1951, "The Catcher in the Rye" },
                    { 6, "John Steinbeck", null, 1939, "The Grapes of Wrath" },
                    { 7, "William Faulkner", null, 1929, "The Sound and the Fury" },
                    { 8, "Toni Morrison", null, 1987, "Beloved" },
                    { 9, "Ralph Ellison", null, 1952, "Invisible Man" },
                    { 10, "Ray Bradbury", null, 1953, "Fahrenheit 451" },
                    { 11, "Kurt Vonnegut", null, 1969, "Slaughterhouse-Five" },
                    { 12, "Jack Kerouac", null, 1957, "On the Road" },
                    { 13, "Ernest Hemingway", null, 1952, "The Old Man and the Sea" },
                    { 14, "John Steinbeck", null, 1952, "East of Eden" },
                    { 15, "John Steinbeck", null, 1937, "Of Mice and Men" },
                    { 16, "Zora Neale Hurston", null, 1937, "Their Eyes Were Watching God" },
                    { 17, "Alice Walker", null, 1982, "The Color Purple" },
                    { 18, "Nathaniel Hawthorne", null, 1850, "The Scarlet Letter" },
                    { 19, "Louisa May Alcott", null, 1868, "Little Women" },
                    { 20, "Harriet Beecher Stowe", null, 1852, "Uncle Tom's Cabin" },
                    { 21, "Walt Whitman", null, 1855, "Leaves of Grass" },
                    { 22, "Ernest Hemingway", null, 1926, "The Sun Also Rises" },
                    { 23, "William Faulkner", null, 1930, "As I Lay Dying" },
                    { 24, "Cormac McCarthy", null, 1985, "Blood Meridian" },
                    { 25, "Cormac McCarthy", null, 2006, "The Road" },
                    { 26, "Jonathan Franzen", null, 2001, "The Corrections" },
                    { 27, "Donna Tartt", null, 2013, "The Goldfinch" },
                    { 28, "Colson Whitehead", null, 2016, "The Underground Railroad" },
                    { 29, "Colson Whitehead", null, 2019, "The Nickel Boys" },
                    { 30, "Don DeLillo", null, 1985, "White Noise" },
                    { 31, "David Foster Wallace", null, 1996, "Infinite Jest" },
                    { 32, "Joseph Heller", null, 1961, "Catch-22" },
                    { 33, "Ken Kesey", null, 1962, "One Flew Over the Cuckoo's Nest" },
                    { 34, "Jack London", null, 1903, "The Call of the Wild" },
                    { 35, "Jack London", null, 1906, "White Fang" },
                    { 36, "Edith Wharton", null, 1905, "The House of Mirth" },
                    { 37, "Edith Wharton", null, 1920, "The Age of Innocence" },
                    { 38, "Upton Sinclair", null, 1906, "The Jungle" },
                    { 39, "Sylvia Plath", null, 1963, "The Bell Jar" },
                    { 40, "Philip Roth", null, 1997, "American Pastoral" },
                    { 41, "Philip Roth", null, 1969, "Portnoy's Complaint" },
                    { 42, "Ursula K. Le Guin", null, 1969, "The Left Hand of Darkness" },
                    { 43, "Frank Herbert", null, 1965, "Dune" },
                    { 44, "Isaac Asimov", null, 1951, "Foundation" },
                    { 45, "Isaac Asimov", null, 1952, "Foundation and Empire" },
                    { 46, "Isaac Asimov", null, 1953, "Second Foundation" },
                    { 47, "Michael Chabon", null, 2000, "The Amazing Adventures of Kavalier & Clay" },
                    { 48, "Michael Chabon", null, 2007, "The Yiddish Policemen's Union" },
                    { 49, "Barbara Kingsolver", null, 1998, "The Poisonwood Bible" },
                    { 50, "Betty Smith", null, 1943, "A Tree Grows in Brooklyn" },
                    { 51, "Margaret Mitchell", null, 1936, "Gone with the Wind" },
                    { 52, "S. E. Hinton", null, 1967, "The Outsiders" },
                    { 53, "Lois Lowry", null, 1993, "The Giver" },
                    { 54, "Tim O'Brien", null, 1990, "The Things They Carried" },
                    { 55, "Richard Wright", null, 1940, "Native Son" },
                    { 56, "Malcolm X; Alex Haley", null, 1965, "The Autobiography of Malcolm X" },
                    { 57, "W. E. B. Du Bois", null, 1903, "The Souls of Black Folk" },
                    { 58, "Mark Twain", null, 1876, "The Adventures of Tom Sawyer" },
                    { 59, "Ray Bradbury", null, 1950, "The Martian Chronicles" },
                    { 60, "Stephen King", null, 1978, "The Stand" },
                    { 61, "Stephen King", null, 1986, "It" },
                    { 62, "Stephen King", null, 1977, "The Shining" },
                    { 63, "Vladimir Nabokov", null, 1955, "Lolita" },
                    { 64, "Thomas Pynchon", null, 1966, "The Crying of Lot 49" },
                    { 65, "Thomas Pynchon", null, 1973, "Gravity's Rainbow" },
                    { 66, "Tom Wolfe", null, 1987, "The Bonfire of the Vanities" },
                    { 67, "Tom Wolfe", null, 1979, "The Right Stuff" },
                    { 68, "John Kennedy Toole", null, 1980, "A Confederacy of Dunces" },
                    { 69, "Junot Díaz", null, 2007, "The Brief Wondrous Life of Oscar Wao" },
                    { 70, "Amy Tan", null, 1989, "The Joy Luck Club" },
                    { 71, "Sandra Cisneros", null, 1984, "The House on Mango Street" },
                    { 72, "James McBride", null, 1995, "The Color of Water" },
                    { 73, "Donna Tartt", null, 1992, "The Secret History" },
                    { 74, "Alice Sebold", null, 2002, "The Lovely Bones" },
                    { 75, "Erik Larson", null, 2003, "The Devil in the White City" },
                    { 76, "Suzanne Collins", null, 2008, "The Hunger Games" },
                    { 77, "John Green", null, 2012, "The Fault in Our Stars" },
                    { 78, "Jeannette Walls", null, 2005, "The Glass Castle" },
                    { 79, "Kathryn Stockett", null, 2009, "The Help" },
                    { 80, "Cormac McCarthy", null, 2005, "No Country for Old Men" },
                    { 81, "Toni Morrison", null, 1970, "The Bluest Eye" },
                    { 82, "Toni Morrison", null, 1977, "Song of Solomon" },
                    { 83, "Jhumpa Lahiri", null, 2003, "The Namesake" },
                    { 84, "Jonathan Franzen", null, 2010, "Freedom" },
                    { 85, "Marilynne Robinson", null, 1980, "Housekeeping" },
                    { 86, "Marilynne Robinson", null, 2004, "Gilead" },
                    { 87, "William Goldman", null, 1973, "The Princess Bride" },
                    { 88, "Mario Puzo", null, 1969, "The Godfather" },
                    { 89, "John Grisham", null, 1991, "The Firm" },
                    { 90, "Dan Brown", null, 2003, "The Da Vinci Code" },
                    { 91, "Nathaniel Hawthorne", null, 1851, "The House of the Seven Gables" },
                    { 92, "James Fenimore Cooper", null, 1826, "The Last of the Mohicans" },
                    { 93, "Sinclair Lewis", null, 1922, "Babbitt" },
                    { 94, "Sinclair Lewis", null, 1920, "Main Street" },
                    { 95, "Stephen Crane", null, 1895, "The Red Badge of Courage" },
                    { 96, "W. H. Auden", null, 1947, "The Age of Anxiety" },
                    { 97, "Isabel Allende", null, 1982, "The House of the Spirits" },
                    { 98, "Arundhati Roy", null, 1997, "The God of Small Things" },
                    { 99, "Erich Maria Remarque", null, 1931, "The Road Back" },
                    { 100, "Philip Roth", null, 2004, "The Plot Against America" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Title_Authors",
                table: "Books",
                columns: new[] { "Title", "Authors" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
