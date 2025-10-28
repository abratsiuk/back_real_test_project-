using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace back_test_project.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DepartmentName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "DepartmentName" },
                values: new object[,]
                {
                    { 1, "Engineering" },
                    { 2, "HR" },
                    { 3, "Finance" },
                    { 4, "Marketing" },
                    { 5, "Sales" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "FirstName", "LastName", "ManagerId", "Salary" },
                values: new object[,]
                {
                    { 1, 1, "Mark", "Twain", null, 3000m },
                    { 7, 4, "Stephen", "King", null, 3800m },
                    { 9, 5, "Edgar", "Poe", null, 3700m },
                    { 2, 1, "Ernest", "Hemingway", 1, 3500m },
                    { 3, 2, "Harper", "Lee", 1, 2000m },
                    { 5, 3, "John", "Steinbeck", 1, 4500m },
                    { 8, 4, "Jack", "London", 7, 2100m },
                    { 10, 5, "Walt", "Whitman", 9, 3050m },
                    { 11, 1, "Tom", "Hanks", 1, 5200m },
                    { 12, 1, "Meryl", "Streep", 1, 5400m },
                    { 14, 1, "Leonardo", "DiCaprio", 7, 5500m },
                    { 15, 1, "Brad", "Pitt", 9, 5300m },
                    { 16, 1, "Angelina", "Jolie", 1, 5200m },
                    { 18, 1, "Al", "Pacino", 7, 5600m },
                    { 19, 1, "Morgan", "Freeman", 9, 5800m },
                    { 20, 1, "Samuel", "Jackson", 1, 5200m },
                    { 22, 1, "Chris", "Evans", 7, 4900m },
                    { 23, 1, "Chris", "Hemsworth", 9, 5000m },
                    { 24, 1, "Robert", "Downey", 1, 5800m },
                    { 26, 1, "Natalie", "Portman", 7, 5150m },
                    { 27, 1, "Matt", "Damon", 9, 5200m },
                    { 28, 1, "Ben", "Affleck", 1, 4900m },
                    { 30, 1, "Harrison", "Ford", 7, 5500m },
                    { 31, 1, "Julia", "Roberts", 9, 5100m },
                    { 32, 1, "Sandra", "Bullock", 1, 5050m },
                    { 34, 1, "Emma", "Stone", 7, 4800m },
                    { 35, 1, "Ryan", "Gosling", 9, 4850m },
                    { 36, 1, "Christian", "Bale", 1, 5400m },
                    { 38, 1, "Will", "Smith", 7, 5200m },
                    { 39, 1, "Joaquin", "Phoenix", 9, 5450m },
                    { 40, 1, "Rami", "Malek", 1, 4700m },
                    { 42, 1, "Aaron", "Paul", 7, 4550m },
                    { 43, 1, "Zoe", "Saldana", 9, 4700m },
                    { 44, 1, "Amy", "Adams", 1, 5100m },
                    { 46, 1, "Octavia", "Spencer", 7, 4800m },
                    { 47, 1, "Melissa", "McCarthy", 9, 4500m },
                    { 48, 1, "Steve", "Carell", 1, 4600m },
                    { 50, 1, "Eddie", "Murphy", 7, 5200m },
                    { 51, 1, "Chris", "Pratt", 9, 4800m },
                    { 52, 1, "Zachary", "Levi", 1, 4550m },
                    { 54, 1, "Brie", "Larson", 7, 4950m },
                    { 55, 1, "Jason", "Momoa", 9, 5000m },
                    { 56, 1, "Mark", "Ruffalo", 1, 5150m },
                    { 58, 1, "Paul", "Rudd", 7, 4800m },
                    { 59, 1, "Chadwick", "Boseman", 9, 5400m },
                    { 60, 1, "Michael", "Keaton", 1, 5100m },
                    { 62, 1, "Nicolas", "Cage", 7, 5000m },
                    { 63, 1, "Keegan-Michael", "Key", 9, 4500m },
                    { 64, 1, "Jordan", "Peele", 1, 5200m },
                    { 66, 1, "Ethan", "Hawke", 7, 4800m },
                    { 67, 1, "Jodie", "Foster", 9, 5300m },
                    { 68, 1, "Sigourney", "Weaver", 1, 5200m },
                    { 70, 1, "Winona", "Ryder", 7, 4700m },
                    { 71, 2, "Michael", "Jordan", 9, 6000m },
                    { 73, 2, "Tiger", "Woods", 7, 5800m },
                    { 74, 2, "LeBron", "James", 1, 5900m },
                    { 75, 2, "Kobe", "Bryant", 9, 6000m },
                    { 77, 2, "Tom", "Brady", 7, 5800m },
                    { 78, 2, "Peyton", "Manning", 1, 5600m },
                    { 79, 2, "Joe", "Montana", 9, 5500m },
                    { 81, 2, "Alex", "Morgan", 7, 5200m },
                    { 82, 2, "Simone", "Biles", 1, 5400m },
                    { 83, 2, "Michael", "Phelps", 9, 5600m },
                    { 85, 2, "Billie", "Jean", 7, 5100m },
                    { 86, 2, "Venus", "Williams", 1, 5450m },
                    { 87, 2, "Larry", "Bird", 9, 5600m },
                    { 89, 2, "Kareem", "AbdulJabbar", 7, 5900m },
                    { 90, 2, "Charles", "Barkley", 1, 5300m },
                    { 91, 2, "Allen", "Iverson", 9, 5200m },
                    { 93, 2, "Kevin", "Durant", 7, 5750m },
                    { 94, 2, "James", "Harden", 1, 5650m },
                    { 95, 2, "Kawhi", "Leonard", 9, 5500m },
                    { 97, 2, "Damian", "Lillard", 7, 5400m },
                    { 98, 2, "Russell", "Westbrook", 1, 5450m },
                    { 99, 2, "Carmelo", "Anthony", 9, 5350m },
                    { 101, 3, "Ernest", "Hemingway", 1, 5200m },
                    { 103, 3, "Ray", "Bradbury", 7, 5000m },
                    { 104, 3, "Kurt", "Vonnegut", 9, 4950m },
                    { 105, 3, "George", "Martin", 1, 5300m },
                    { 107, 3, "Philip", "Dick", 7, 5050m },
                    { 108, 3, "Ursula", "LeGuin", 9, 5200m },
                    { 109, 3, "James", "Baldwin", 1, 5100m },
                    { 111, 3, "Truman", "Capote", 7, 5000m },
                    { 112, 3, "JD", "Salinger", 9, 5050m },
                    { 113, 3, "Thomas", "Pynchon", 1, 5300m },
                    { 115, 3, "Cormac", "McCarthy", 7, 5400m },
                    { 116, 3, "Jonathan", "Franzen", 9, 4850m },
                    { 117, 3, "Zadie", "Smith", 1, 4800m },
                    { 119, 3, "Colson", "Whitehead", 7, 4950m },
                    { 120, 3, "Jhumpa", "Lahiri", 9, 4750m },
                    { 121, 3, "Stephen", "Curry", 1, 4600m },
                    { 123, 3, "Maya", "Angelou", 7, 5100m },
                    { 124, 3, "Tennessee", "Williams", 9, 5200m },
                    { 125, 3, "Arthur", "Miller", 1, 5050m },
                    { 127, 3, "Alice", "Walker", 7, 4900m },
                    { 128, 3, "Amy", "Tan", 9, 4700m },
                    { 129, 3, "Neil", "Gaiman", 1, 5200m },
                    { 131, 3, "Michael", "Chabon", 7, 4850m },
                    { 132, 3, "Harlan", "Ellison", 9, 4950m },
                    { 133, 3, "Anne", "Tyler", 1, 4650m },
                    { 135, 3, "TaNehisi", "Coates", 7, 4800m },
                    { 136, 3, "Roxane", "Gay", 9, 4600m },
                    { 137, 3, "Walter", "Mosley", 1, 5000m },
                    { 139, 3, "Nassim", "Taleb", 7, 5400m },
                    { 140, 3, "Steven", "Levitt", 9, 5350m },
                    { 141, 4, "Clint", "Eastwood", 7, 5600m },
                    { 142, 4, "Quentin", "Tarantino", 1, 5500m },
                    { 144, 4, "Martin", "Scorsese", 9, 5800m },
                    { 145, 4, "Spike", "Lee", 7, 5200m },
                    { 146, 4, "Greta", "Gerwig", 1, 5150m },
                    { 148, 4, "Sofia", "Coppola", 9, 5050m },
                    { 149, 4, "Wes", "Anderson", 7, 5000m },
                    { 150, 4, "David", "Fincher", 1, 5450m },
                    { 152, 4, "James", "Cameron", 9, 5900m },
                    { 153, 4, "JJ", "Abrams", 7, 5300m },
                    { 154, 4, "Ridley", "Scott", 1, 5550m },
                    { 156, 4, "Jordan", "Peele", 9, 5200m },
                    { 157, 4, "Taika", "Waititi", 7, 5150m },
                    { 158, 4, "Patty", "Jenkins", 1, 5100m },
                    { 160, 4, "Ryan", "Coogler", 9, 5250m },
                    { 161, 4, "Jon", "Favreau", 7, 5350m },
                    { 162, 4, "Barry", "Jenkins", 1, 5200m },
                    { 164, 4, "Noah", "Baumbach", 9, 5050m },
                    { 165, 4, "David", "Lynch", 7, 5400m },
                    { 166, 5, "Stephen", "King", 9, 5800m },
                    { 167, 5, "Mark", "Twain", 1, 5600m },
                    { 168, 5, "Edgar", "Allan", 9, 5400m },
                    { 169, 5, "Walt", "Whitman", 9, 5200m },
                    { 171, 5, "Nathaniel", "Hawthorne", 7, 5050m },
                    { 172, 5, "Sylvia", "Plath", 1, 4750m },
                    { 174, 5, "Louisa", "Alcott", 7, 4650m },
                    { 175, 5, "Harper", "Lee", 1, 4900m },
                    { 176, 5, "Jack", "Kerouac", 9, 5000m },
                    { 178, 5, "Tracy", "McGrady", 7, 5200m },
                    { 179, 5, "Reggie", "Miller", 1, 5150m },
                    { 180, 5, "Patrick", "Ewing", 9, 5250m },
                    { 182, 5, "John", "Stockton", 7, 5200m },
                    { 183, 5, "Hakeem", "Olajuwon", 1, 5450m },
                    { 184, 5, "Tim", "Duncan", 9, 5550m },
                    { 186, 5, "Dwyane", "Wade", 7, 5450m },
                    { 187, 5, "Chris", "Paul", 1, 5350m },
                    { 188, 5, "Klay", "Thompson", 9, 5400m },
                    { 190, 5, "Devin", "Booker", 7, 5150m },
                    { 191, 5, "Jayson", "Tatum", 1, 5350m },
                    { 192, 5, "Jaylen", "Brown", 9, 5200m },
                    { 194, 5, "Luka", "Doncic", 7, 5850m },
                    { 195, 5, "Jimmy", "Butler", 1, 5450m },
                    { 196, 5, "Kyrie", "Irving", 9, 5550m },
                    { 198, 5, "Kemba", "Walker", 7, 5100m },
                    { 199, 5, "Zion", "Williamson", 1, 5600m },
                    { 200, 5, "Ja", "Morant", 9, 5450m },
                    { 4, 2, "F. Scott", "Fitzgerald", 3, 2700m },
                    { 6, 3, "Toni", "Morrison", 5, 3200m },
                    { 13, 1, "Denzel", "Washington", 5, 5600m },
                    { 17, 1, "Robert", "DeNiro", 5, 5700m },
                    { 21, 1, "Scarlett", "Johansson", 5, 5100m },
                    { 25, 1, "Jennifer", "Lawrence", 5, 5050m },
                    { 29, 1, "Keanu", "Reeves", 5, 5400m },
                    { 33, 1, "Anne", "Hathaway", 5, 4950m },
                    { 37, 1, "Hugh", "Jackman", 5, 5350m },
                    { 41, 1, "Bryan", "Cranston", 5, 5000m },
                    { 45, 1, "Viola", "Davis", 5, 5200m },
                    { 49, 1, "Jim", "Carrey", 5, 5000m },
                    { 53, 1, "Gal", "Gadot", 5, 5050m },
                    { 57, 1, "Jeremy", "Renner", 5, 4700m },
                    { 61, 1, "Tom", "Cruise", 5, 5600m },
                    { 65, 1, "Kerry", "Washington", 5, 4950m },
                    { 69, 1, "Uma", "Thurman", 5, 4850m },
                    { 72, 2, "Serena", "Williams", 5, 5900m },
                    { 76, 2, "Shaquille", "ONeal", 5, 5700m },
                    { 80, 2, "Mia", "Hamm", 5, 5200m },
                    { 84, 2, "Lindsey", "Vonn", 5, 5200m },
                    { 88, 2, "Magic", "Johnson", 5, 5800m },
                    { 92, 2, "Steph", "Curry", 5, 5850m },
                    { 96, 2, "Giannis", "Antetokounmpo", 5, 5600m },
                    { 100, 2, "Dirk", "Nowitzki", 5, 5700m },
                    { 102, 3, "William", "Faulkner", 5, 5100m },
                    { 106, 3, "Isaac", "Asimov", 5, 5400m },
                    { 110, 3, "Ralph", "Ellison", 5, 4950m },
                    { 114, 3, "Don", "DeLillo", 5, 5200m },
                    { 118, 3, "Donna", "Tartt", 5, 4900m },
                    { 122, 3, "Serena", "Williams", 5, 4700m },
                    { 126, 3, "Tony", "Morrison", 5, 4950m },
                    { 130, 3, "Suzanne", "Collins", 5, 4750m },
                    { 134, 3, "Donna", "Haraway", 5, 4700m },
                    { 138, 3, "Michael", "Lewis", 5, 5450m },
                    { 143, 4, "Christopher", "Nolan", 5, 5700m },
                    { 147, 4, "Ava", "DuVernay", 5, 5100m },
                    { 151, 4, "Ron", "Howard", 5, 5200m },
                    { 155, 4, "Denis", "Villeneuve", 5, 5600m },
                    { 159, 4, "Catherine", "Hardwicke", 5, 4850m },
                    { 163, 4, "Seth", "MacFarlane", 5, 5000m },
                    { 170, 5, "Herman", "Melville", 5, 5100m },
                    { 173, 5, "Emily", "Dickinson", 5, 4800m },
                    { 177, 5, "Allen", "Ginsberg", 5, 4850m },
                    { 181, 5, "Karl", "Malone", 5, 5300m },
                    { 185, 5, "Dirk", "Nowitzki", 5, 5400m },
                    { 189, 5, "Draymond", "Green", 5, 5200m },
                    { 193, 5, "Nikola", "Jokic", 5, 5900m },
                    { 197, 5, "Paul", "George", 5, 5400m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentName",
                table: "Departments",
                column: "DepartmentName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ManagerId",
                table: "Employees",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
