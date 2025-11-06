using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Cat> Cats => Set<Cat>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Department>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.DepartmentName).IsRequired().HasMaxLength(200);
                e.HasIndex(x => x.DepartmentName).IsUnique();
            });

            b.Entity<Employee>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
                e.Property(x => x.LastName).IsRequired().HasMaxLength(100);

                //e.Property(x => x.Salary).HasColumnType("decimal(18,2)");
                e.Property(x => x.Salary).HasPrecision(18, 2);

                // prevent cascade delete
                e.HasOne(x => x.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(x => x.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                // prevent deleting manager with subs
                e.HasOne(x => x.Manager)
                    .WithMany(m => m!.Subordinates)
                    .HasForeignKey(x => x.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict);

                //// prevent to be manager for him self
                //e.ToTable(t =>
                //{
                //    t.HasCheckConstraint("CK_Employee_Manager_Not_Self", "[ManagerId] IS NULL OR [ManagerId] <> [Id]");
                //});

            });
            b.Entity<Book>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Title).IsRequired().HasMaxLength(300);
                e.Property(x => x.Authors).IsRequired().HasMaxLength(400);

                // Description is optional long text
                e.Property(x => x.Description).HasMaxLength(4000);

                // Basic index for search/sort; keep unique only on (Title, Authors)
                e.HasIndex(x => new { x.Title, x.Authors }).IsUnique();
            });

            b.Entity<Cat>(e =>
            {
                e.HasKey(x => x.Id);

                e.Property(x => x.Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.Age).IsRequired();
            });

            b.Entity<Book>().HasData(
    new Book { Id = 1, Title = "Moby-Dick", Authors = "Herman Melville", PublicationYear = 1851, Description = null },
    new Book { Id = 2, Title = "The Adventures of Huckleberry Finn", Authors = "Mark Twain", PublicationYear = 1884, Description = null },
    new Book { Id = 3, Title = "The Great Gatsby", Authors = "F. Scott Fitzgerald", PublicationYear = 1925, Description = null },
    new Book { Id = 4, Title = "To Kill a Mockingbird", Authors = "Harper Lee", PublicationYear = 1960, Description = null },
    new Book { Id = 5, Title = "The Catcher in the Rye", Authors = "J. D. Salinger", PublicationYear = 1951, Description = null },
    new Book { Id = 6, Title = "The Grapes of Wrath", Authors = "John Steinbeck", PublicationYear = 1939, Description = null },
    new Book { Id = 7, Title = "The Sound and the Fury", Authors = "William Faulkner", PublicationYear = 1929, Description = null },
    new Book { Id = 8, Title = "Beloved", Authors = "Toni Morrison", PublicationYear = 1987, Description = null },
    new Book { Id = 9, Title = "Invisible Man", Authors = "Ralph Ellison", PublicationYear = 1952, Description = null },
    new Book { Id = 10, Title = "Fahrenheit 451", Authors = "Ray Bradbury", PublicationYear = 1953, Description = null },
    new Book { Id = 11, Title = "Slaughterhouse-Five", Authors = "Kurt Vonnegut", PublicationYear = 1969, Description = null },
    new Book { Id = 12, Title = "On the Road", Authors = "Jack Kerouac", PublicationYear = 1957, Description = null },
    new Book { Id = 13, Title = "The Old Man and the Sea", Authors = "Ernest Hemingway", PublicationYear = 1952, Description = null },
    new Book { Id = 14, Title = "East of Eden", Authors = "John Steinbeck", PublicationYear = 1952, Description = null },
    new Book { Id = 15, Title = "Of Mice and Men", Authors = "John Steinbeck", PublicationYear = 1937, Description = null },
    new Book { Id = 16, Title = "Their Eyes Were Watching God", Authors = "Zora Neale Hurston", PublicationYear = 1937, Description = null },
    new Book { Id = 17, Title = "The Color Purple", Authors = "Alice Walker", PublicationYear = 1982, Description = null },
    new Book { Id = 18, Title = "The Scarlet Letter", Authors = "Nathaniel Hawthorne", PublicationYear = 1850, Description = null },
    new Book { Id = 19, Title = "Little Women", Authors = "Louisa May Alcott", PublicationYear = 1868, Description = null },
    new Book { Id = 20, Title = "Uncle Tom's Cabin", Authors = "Harriet Beecher Stowe", PublicationYear = 1852, Description = null },
    new Book { Id = 21, Title = "Leaves of Grass", Authors = "Walt Whitman", PublicationYear = 1855, Description = null },
    new Book { Id = 22, Title = "The Sun Also Rises", Authors = "Ernest Hemingway", PublicationYear = 1926, Description = null },
    new Book { Id = 23, Title = "As I Lay Dying", Authors = "William Faulkner", PublicationYear = 1930, Description = null },
    new Book { Id = 24, Title = "Blood Meridian", Authors = "Cormac McCarthy", PublicationYear = 1985, Description = null },
    new Book { Id = 25, Title = "The Road", Authors = "Cormac McCarthy", PublicationYear = 2006, Description = null },
    new Book { Id = 26, Title = "The Corrections", Authors = "Jonathan Franzen", PublicationYear = 2001, Description = null },
    new Book { Id = 27, Title = "The Goldfinch", Authors = "Donna Tartt", PublicationYear = 2013, Description = null },
    new Book { Id = 28, Title = "The Underground Railroad", Authors = "Colson Whitehead", PublicationYear = 2016, Description = null },
    new Book { Id = 29, Title = "The Nickel Boys", Authors = "Colson Whitehead", PublicationYear = 2019, Description = null },
    new Book { Id = 30, Title = "White Noise", Authors = "Don DeLillo", PublicationYear = 1985, Description = null },
    new Book { Id = 31, Title = "Infinite Jest", Authors = "David Foster Wallace", PublicationYear = 1996, Description = null },
    new Book { Id = 32, Title = "Catch-22", Authors = "Joseph Heller", PublicationYear = 1961, Description = null },
    new Book { Id = 33, Title = "One Flew Over the Cuckoo's Nest", Authors = "Ken Kesey", PublicationYear = 1962, Description = null },
    new Book { Id = 34, Title = "The Call of the Wild", Authors = "Jack London", PublicationYear = 1903, Description = null },
    new Book { Id = 35, Title = "White Fang", Authors = "Jack London", PublicationYear = 1906, Description = null },
    new Book { Id = 36, Title = "The House of Mirth", Authors = "Edith Wharton", PublicationYear = 1905, Description = null },
    new Book { Id = 37, Title = "The Age of Innocence", Authors = "Edith Wharton", PublicationYear = 1920, Description = null },
    new Book { Id = 38, Title = "The Jungle", Authors = "Upton Sinclair", PublicationYear = 1906, Description = null },
    new Book { Id = 39, Title = "The Bell Jar", Authors = "Sylvia Plath", PublicationYear = 1963, Description = null },
    new Book { Id = 40, Title = "American Pastoral", Authors = "Philip Roth", PublicationYear = 1997, Description = null },
    new Book { Id = 41, Title = "Portnoy's Complaint", Authors = "Philip Roth", PublicationYear = 1969, Description = null },
    new Book { Id = 42, Title = "The Left Hand of Darkness", Authors = "Ursula K. Le Guin", PublicationYear = 1969, Description = null },
    new Book { Id = 43, Title = "Dune", Authors = "Frank Herbert", PublicationYear = 1965, Description = null },
    new Book { Id = 44, Title = "Foundation", Authors = "Isaac Asimov", PublicationYear = 1951, Description = null },
    new Book { Id = 45, Title = "Foundation and Empire", Authors = "Isaac Asimov", PublicationYear = 1952, Description = null },
    new Book { Id = 46, Title = "Second Foundation", Authors = "Isaac Asimov", PublicationYear = 1953, Description = null },
    new Book { Id = 47, Title = "The Amazing Adventures of Kavalier & Clay", Authors = "Michael Chabon", PublicationYear = 2000, Description = null },
    new Book { Id = 48, Title = "The Yiddish Policemen's Union", Authors = "Michael Chabon", PublicationYear = 2007, Description = null },
    new Book { Id = 49, Title = "The Poisonwood Bible", Authors = "Barbara Kingsolver", PublicationYear = 1998, Description = null },
    new Book { Id = 50, Title = "A Tree Grows in Brooklyn", Authors = "Betty Smith", PublicationYear = 1943, Description = null },
    new Book { Id = 51, Title = "Gone with the Wind", Authors = "Margaret Mitchell", PublicationYear = 1936, Description = null },
    new Book { Id = 52, Title = "The Outsiders", Authors = "S. E. Hinton", PublicationYear = 1967, Description = null },
    new Book { Id = 53, Title = "The Giver", Authors = "Lois Lowry", PublicationYear = 1993, Description = null },
    new Book { Id = 54, Title = "The Things They Carried", Authors = "Tim O'Brien", PublicationYear = 1990, Description = null },
    new Book { Id = 55, Title = "Native Son", Authors = "Richard Wright", PublicationYear = 1940, Description = null },
    new Book { Id = 56, Title = "The Autobiography of Malcolm X", Authors = "Malcolm X; Alex Haley", PublicationYear = 1965, Description = null },
    new Book { Id = 57, Title = "The Souls of Black Folk", Authors = "W. E. B. Du Bois", PublicationYear = 1903, Description = null },
    new Book { Id = 58, Title = "The Adventures of Tom Sawyer", Authors = "Mark Twain", PublicationYear = 1876, Description = null },
    new Book { Id = 59, Title = "The Martian Chronicles", Authors = "Ray Bradbury", PublicationYear = 1950, Description = null },
    new Book { Id = 60, Title = "The Stand", Authors = "Stephen King", PublicationYear = 1978, Description = null },
    new Book { Id = 61, Title = "It", Authors = "Stephen King", PublicationYear = 1986, Description = null },
    new Book { Id = 62, Title = "The Shining", Authors = "Stephen King", PublicationYear = 1977, Description = null },
    new Book { Id = 63, Title = "Lolita", Authors = "Vladimir Nabokov", PublicationYear = 1955, Description = null },
    new Book { Id = 64, Title = "The Crying of Lot 49", Authors = "Thomas Pynchon", PublicationYear = 1966, Description = null },
    new Book { Id = 65, Title = "Gravity's Rainbow", Authors = "Thomas Pynchon", PublicationYear = 1973, Description = null },
    new Book { Id = 66, Title = "The Bonfire of the Vanities", Authors = "Tom Wolfe", PublicationYear = 1987, Description = null },
    new Book { Id = 67, Title = "The Right Stuff", Authors = "Tom Wolfe", PublicationYear = 1979, Description = null },
    new Book { Id = 68, Title = "A Confederacy of Dunces", Authors = "John Kennedy Toole", PublicationYear = 1980, Description = null },
    new Book { Id = 69, Title = "The Brief Wondrous Life of Oscar Wao", Authors = "Junot Díaz", PublicationYear = 2007, Description = null },
    new Book { Id = 70, Title = "The Joy Luck Club", Authors = "Amy Tan", PublicationYear = 1989, Description = null },
    new Book { Id = 71, Title = "The House on Mango Street", Authors = "Sandra Cisneros", PublicationYear = 1984, Description = null },
    new Book { Id = 72, Title = "The Color of Water", Authors = "James McBride", PublicationYear = 1995, Description = null },
    new Book { Id = 73, Title = "The Secret History", Authors = "Donna Tartt", PublicationYear = 1992, Description = null },
    new Book { Id = 74, Title = "The Lovely Bones", Authors = "Alice Sebold", PublicationYear = 2002, Description = null },
    new Book { Id = 75, Title = "The Devil in the White City", Authors = "Erik Larson", PublicationYear = 2003, Description = null },
    new Book { Id = 76, Title = "The Hunger Games", Authors = "Suzanne Collins", PublicationYear = 2008, Description = null },
    new Book { Id = 77, Title = "The Fault in Our Stars", Authors = "John Green", PublicationYear = 2012, Description = null },
    new Book { Id = 78, Title = "The Glass Castle", Authors = "Jeannette Walls", PublicationYear = 2005, Description = null },
    new Book { Id = 79, Title = "The Help", Authors = "Kathryn Stockett", PublicationYear = 2009, Description = null },
    new Book { Id = 80, Title = "No Country for Old Men", Authors = "Cormac McCarthy", PublicationYear = 2005, Description = null },
    new Book { Id = 81, Title = "The Bluest Eye", Authors = "Toni Morrison", PublicationYear = 1970, Description = null },
    new Book { Id = 82, Title = "Song of Solomon", Authors = "Toni Morrison", PublicationYear = 1977, Description = null },
    new Book { Id = 83, Title = "The Namesake", Authors = "Jhumpa Lahiri", PublicationYear = 2003, Description = null },
    new Book { Id = 84, Title = "Freedom", Authors = "Jonathan Franzen", PublicationYear = 2010, Description = null },
    new Book { Id = 85, Title = "Housekeeping", Authors = "Marilynne Robinson", PublicationYear = 1980, Description = null },
    new Book { Id = 86, Title = "Gilead", Authors = "Marilynne Robinson", PublicationYear = 2004, Description = null },
    new Book { Id = 87, Title = "The Princess Bride", Authors = "William Goldman", PublicationYear = 1973, Description = null },
    new Book { Id = 88, Title = "The Godfather", Authors = "Mario Puzo", PublicationYear = 1969, Description = null },
    new Book { Id = 89, Title = "The Firm", Authors = "John Grisham", PublicationYear = 1991, Description = null },
    new Book { Id = 90, Title = "The Da Vinci Code", Authors = "Dan Brown", PublicationYear = 2003, Description = null },
    new Book { Id = 91, Title = "The House of the Seven Gables", Authors = "Nathaniel Hawthorne", PublicationYear = 1851, Description = null },
    new Book { Id = 92, Title = "The Last of the Mohicans", Authors = "James Fenimore Cooper", PublicationYear = 1826, Description = null },
    new Book { Id = 93, Title = "Babbitt", Authors = "Sinclair Lewis", PublicationYear = 1922, Description = null },
    new Book { Id = 94, Title = "Main Street", Authors = "Sinclair Lewis", PublicationYear = 1920, Description = null },
    new Book { Id = 95, Title = "The Red Badge of Courage", Authors = "Stephen Crane", PublicationYear = 1895, Description = null },
    new Book { Id = 96, Title = "The Age of Anxiety", Authors = "W. H. Auden", PublicationYear = 1947, Description = null },
    new Book { Id = 97, Title = "The House of the Spirits", Authors = "Isabel Allende", PublicationYear = 1982, Description = null },
    new Book { Id = 98, Title = "The God of Small Things", Authors = "Arundhati Roy", PublicationYear = 1997, Description = null },
    new Book { Id = 99, Title = "The Road Back", Authors = "Erich Maria Remarque", PublicationYear = 1931, Description = null },
    new Book { Id = 100, Title = "The Plot Against America", Authors = "Philip Roth", PublicationYear = 2004, Description = null }
);

            // ---- Seed data (5 departments, 10 employees) ----
            // Departments
            b.Entity<Department>().HasData(
                new Department { Id = 1, DepartmentName = "Engineering" },
                new Department { Id = 2, DepartmentName = "HR" },
                new Department { Id = 3, DepartmentName = "Finance" },
                new Department { Id = 4, DepartmentName = "Marketing" },
                new Department { Id = 5, DepartmentName = "Sales" }
            );

            // Employees (per department: 1 manager with higher salary, 1 subordinate)
            // D1: Engineering
            b.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Mark", LastName = "Twain", Salary = 3000m, DepartmentId = 1, ManagerId = null },
                new Employee { Id = 2, FirstName = "Ernest", LastName = "Hemingway", Salary = 3500m, DepartmentId = 1, ManagerId = 1 },

                // D2: HR
                //manager from another department - "Mark Twain" is manager for Harper Lee:
                new Employee { Id = 3, FirstName = "Harper", LastName = "Lee", Salary = 2000m, DepartmentId = 2, ManagerId = 1 },
                new Employee { Id = 4, FirstName = "F. Scott", LastName = "Fitzgerald", Salary = 2700m, DepartmentId = 2, ManagerId = 3 },

                // D3: Finance
                //manager from another department - "Mark Twain" is manager for John Steinbeck:
                new Employee { Id = 5, FirstName = "John", LastName = "Steinbeck", Salary = 4500m, DepartmentId = 3, ManagerId = 1 },
                new Employee { Id = 6, FirstName = "Toni", LastName = "Morrison", Salary = 3200m, DepartmentId = 3, ManagerId = 5 },

                // D4: Marketing
                new Employee { Id = 7, FirstName = "Stephen", LastName = "King", Salary = 3800m, DepartmentId = 4, ManagerId = null },
                new Employee { Id = 8, FirstName = "Jack", LastName = "London", Salary = 2100m, DepartmentId = 4, ManagerId = 7 },

                // D5: Sales
                new Employee { Id = 9, FirstName = "Edgar", LastName = "Poe", Salary = 3700m, DepartmentId = 5, ManagerId = null },
                new Employee { Id = 10, FirstName = "Walt", LastName = "Whitman", Salary = 3050m, DepartmentId = 5, ManagerId = 9 },

    // Engineering (Id 11–70)
    new Employee { Id = 11, FirstName = "Tom", LastName = "Hanks", Salary = 5200m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 12, FirstName = "Meryl", LastName = "Streep", Salary = 5400m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 13, FirstName = "Denzel", LastName = "Washington", Salary = 5600m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 14, FirstName = "Leonardo", LastName = "DiCaprio", Salary = 5500m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 15, FirstName = "Brad", LastName = "Pitt", Salary = 5300m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 16, FirstName = "Angelina", LastName = "Jolie", Salary = 5200m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 17, FirstName = "Robert", LastName = "DeNiro", Salary = 5700m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 18, FirstName = "Al", LastName = "Pacino", Salary = 5600m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 19, FirstName = "Morgan", LastName = "Freeman", Salary = 5800m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 20, FirstName = "Samuel", LastName = "Jackson", Salary = 5200m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 21, FirstName = "Scarlett", LastName = "Johansson", Salary = 5100m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 22, FirstName = "Chris", LastName = "Evans", Salary = 4900m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 23, FirstName = "Chris", LastName = "Hemsworth", Salary = 5000m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 24, FirstName = "Robert", LastName = "Downey", Salary = 5800m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 25, FirstName = "Jennifer", LastName = "Lawrence", Salary = 5050m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 26, FirstName = "Natalie", LastName = "Portman", Salary = 5150m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 27, FirstName = "Matt", LastName = "Damon", Salary = 5200m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 28, FirstName = "Ben", LastName = "Affleck", Salary = 4900m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 29, FirstName = "Keanu", LastName = "Reeves", Salary = 5400m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 30, FirstName = "Harrison", LastName = "Ford", Salary = 5500m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 31, FirstName = "Julia", LastName = "Roberts", Salary = 5100m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 32, FirstName = "Sandra", LastName = "Bullock", Salary = 5050m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 33, FirstName = "Anne", LastName = "Hathaway", Salary = 4950m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 34, FirstName = "Emma", LastName = "Stone", Salary = 4800m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 35, FirstName = "Ryan", LastName = "Gosling", Salary = 4850m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 36, FirstName = "Christian", LastName = "Bale", Salary = 5400m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 37, FirstName = "Hugh", LastName = "Jackman", Salary = 5350m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 38, FirstName = "Will", LastName = "Smith", Salary = 5200m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 39, FirstName = "Joaquin", LastName = "Phoenix", Salary = 5450m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 40, FirstName = "Rami", LastName = "Malek", Salary = 4700m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 41, FirstName = "Bryan", LastName = "Cranston", Salary = 5000m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 42, FirstName = "Aaron", LastName = "Paul", Salary = 4550m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 43, FirstName = "Zoe", LastName = "Saldana", Salary = 4700m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 44, FirstName = "Amy", LastName = "Adams", Salary = 5100m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 45, FirstName = "Viola", LastName = "Davis", Salary = 5200m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 46, FirstName = "Octavia", LastName = "Spencer", Salary = 4800m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 47, FirstName = "Melissa", LastName = "McCarthy", Salary = 4500m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 48, FirstName = "Steve", LastName = "Carell", Salary = 4600m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 49, FirstName = "Jim", LastName = "Carrey", Salary = 5000m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 50, FirstName = "Eddie", LastName = "Murphy", Salary = 5200m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 51, FirstName = "Chris", LastName = "Pratt", Salary = 4800m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 52, FirstName = "Zachary", LastName = "Levi", Salary = 4550m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 53, FirstName = "Gal", LastName = "Gadot", Salary = 5050m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 54, FirstName = "Brie", LastName = "Larson", Salary = 4950m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 55, FirstName = "Jason", LastName = "Momoa", Salary = 5000m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 56, FirstName = "Mark", LastName = "Ruffalo", Salary = 5150m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 57, FirstName = "Jeremy", LastName = "Renner", Salary = 4700m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 58, FirstName = "Paul", LastName = "Rudd", Salary = 4800m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 59, FirstName = "Chadwick", LastName = "Boseman", Salary = 5400m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 60, FirstName = "Michael", LastName = "Keaton", Salary = 5100m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 61, FirstName = "Tom", LastName = "Cruise", Salary = 5600m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 62, FirstName = "Nicolas", LastName = "Cage", Salary = 5000m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 63, FirstName = "Keegan-Michael", LastName = "Key", Salary = 4500m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 64, FirstName = "Jordan", LastName = "Peele", Salary = 5200m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 65, FirstName = "Kerry", LastName = "Washington", Salary = 4950m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 66, FirstName = "Ethan", LastName = "Hawke", Salary = 4800m, DepartmentId = 1, ManagerId = 7 },
    new Employee { Id = 67, FirstName = "Jodie", LastName = "Foster", Salary = 5300m, DepartmentId = 1, ManagerId = 9 },
    new Employee { Id = 68, FirstName = "Sigourney", LastName = "Weaver", Salary = 5200m, DepartmentId = 1, ManagerId = 1 },
    new Employee { Id = 69, FirstName = "Uma", LastName = "Thurman", Salary = 4850m, DepartmentId = 1, ManagerId = 5 },
    new Employee { Id = 70, FirstName = "Winona", LastName = "Ryder", Salary = 4700m, DepartmentId = 1, ManagerId = 7 },

    // HR (Id 71–100)
    new Employee { Id = 71, FirstName = "Michael", LastName = "Jordan", Salary = 6000m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 72, FirstName = "Serena", LastName = "Williams", Salary = 5900m, DepartmentId = 2, ManagerId = 5 },
    new Employee { Id = 73, FirstName = "Tiger", LastName = "Woods", Salary = 5800m, DepartmentId = 2, ManagerId = 7 },
    new Employee { Id = 74, FirstName = "LeBron", LastName = "James", Salary = 5900m, DepartmentId = 2, ManagerId = 1 },
    new Employee { Id = 75, FirstName = "Kobe", LastName = "Bryant", Salary = 6000m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 76, FirstName = "Shaquille", LastName = "ONeal", Salary = 5700m, DepartmentId = 2, ManagerId = 5 },
    new Employee { Id = 77, FirstName = "Tom", LastName = "Brady", Salary = 5800m, DepartmentId = 2, ManagerId = 7 },
    new Employee { Id = 78, FirstName = "Peyton", LastName = "Manning", Salary = 5600m, DepartmentId = 2, ManagerId = 1 },
    new Employee { Id = 79, FirstName = "Joe", LastName = "Montana", Salary = 5500m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 80, FirstName = "Mia", LastName = "Hamm", Salary = 5200m, DepartmentId = 2, ManagerId = 5 },
    new Employee { Id = 81, FirstName = "Alex", LastName = "Morgan", Salary = 5200m, DepartmentId = 2, ManagerId = 7 },
    new Employee { Id = 82, FirstName = "Simone", LastName = "Biles", Salary = 5400m, DepartmentId = 2, ManagerId = 1 },
    new Employee { Id = 83, FirstName = "Michael", LastName = "Phelps", Salary = 5600m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 84, FirstName = "Lindsey", LastName = "Vonn", Salary = 5200m, DepartmentId = 2, ManagerId = 5 },
    new Employee { Id = 85, FirstName = "Billie", LastName = "Jean", Salary = 5100m, DepartmentId = 2, ManagerId = 7 },
    new Employee { Id = 86, FirstName = "Venus", LastName = "Williams", Salary = 5450m, DepartmentId = 2, ManagerId = 1 },
    new Employee { Id = 87, FirstName = "Larry", LastName = "Bird", Salary = 5600m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 88, FirstName = "Magic", LastName = "Johnson", Salary = 5800m, DepartmentId = 2, ManagerId = 5 },
    new Employee { Id = 89, FirstName = "Kareem", LastName = "AbdulJabbar", Salary = 5900m, DepartmentId = 2, ManagerId = 7 },
    new Employee { Id = 90, FirstName = "Charles", LastName = "Barkley", Salary = 5300m, DepartmentId = 2, ManagerId = 1 },
    new Employee { Id = 91, FirstName = "Allen", LastName = "Iverson", Salary = 5200m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 92, FirstName = "Steph", LastName = "Curry", Salary = 5850m, DepartmentId = 2, ManagerId = 5 },
    new Employee { Id = 93, FirstName = "Kevin", LastName = "Durant", Salary = 5750m, DepartmentId = 2, ManagerId = 7 },
    new Employee { Id = 94, FirstName = "James", LastName = "Harden", Salary = 5650m, DepartmentId = 2, ManagerId = 1 },
    new Employee { Id = 95, FirstName = "Kawhi", LastName = "Leonard", Salary = 5500m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 96, FirstName = "Giannis", LastName = "Antetokounmpo", Salary = 5600m, DepartmentId = 2, ManagerId = 5 },
    new Employee { Id = 97, FirstName = "Damian", LastName = "Lillard", Salary = 5400m, DepartmentId = 2, ManagerId = 7 },
    new Employee { Id = 98, FirstName = "Russell", LastName = "Westbrook", Salary = 5450m, DepartmentId = 2, ManagerId = 1 },
    new Employee { Id = 99, FirstName = "Carmelo", LastName = "Anthony", Salary = 5350m, DepartmentId = 2, ManagerId = 9 },
    new Employee { Id = 100, FirstName = "Dirk", LastName = "Nowitzki", Salary = 5700m, DepartmentId = 2, ManagerId = 5 },

    // Finance (Id 101–140)
    new Employee { Id = 101, FirstName = "Ernest", LastName = "Hemingway", Salary = 5200m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 102, FirstName = "William", LastName = "Faulkner", Salary = 5100m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 103, FirstName = "Ray", LastName = "Bradbury", Salary = 5000m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 104, FirstName = "Kurt", LastName = "Vonnegut", Salary = 4950m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 105, FirstName = "George", LastName = "Martin", Salary = 5300m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 106, FirstName = "Isaac", LastName = "Asimov", Salary = 5400m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 107, FirstName = "Philip", LastName = "Dick", Salary = 5050m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 108, FirstName = "Ursula", LastName = "LeGuin", Salary = 5200m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 109, FirstName = "James", LastName = "Baldwin", Salary = 5100m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 110, FirstName = "Ralph", LastName = "Ellison", Salary = 4950m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 111, FirstName = "Truman", LastName = "Capote", Salary = 5000m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 112, FirstName = "JD", LastName = "Salinger", Salary = 5050m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 113, FirstName = "Thomas", LastName = "Pynchon", Salary = 5300m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 114, FirstName = "Don", LastName = "DeLillo", Salary = 5200m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 115, FirstName = "Cormac", LastName = "McCarthy", Salary = 5400m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 116, FirstName = "Jonathan", LastName = "Franzen", Salary = 4850m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 117, FirstName = "Zadie", LastName = "Smith", Salary = 4800m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 118, FirstName = "Donna", LastName = "Tartt", Salary = 4900m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 119, FirstName = "Colson", LastName = "Whitehead", Salary = 4950m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 120, FirstName = "Jhumpa", LastName = "Lahiri", Salary = 4750m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 121, FirstName = "Stephen", LastName = "Curry", Salary = 4600m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 122, FirstName = "Serena", LastName = "Williams", Salary = 4700m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 123, FirstName = "Maya", LastName = "Angelou", Salary = 5100m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 124, FirstName = "Tennessee", LastName = "Williams", Salary = 5200m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 125, FirstName = "Arthur", LastName = "Miller", Salary = 5050m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 126, FirstName = "Tony", LastName = "Morrison", Salary = 4950m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 127, FirstName = "Alice", LastName = "Walker", Salary = 4900m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 128, FirstName = "Amy", LastName = "Tan", Salary = 4700m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 129, FirstName = "Neil", LastName = "Gaiman", Salary = 5200m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 130, FirstName = "Suzanne", LastName = "Collins", Salary = 4750m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 131, FirstName = "Michael", LastName = "Chabon", Salary = 4850m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 132, FirstName = "Harlan", LastName = "Ellison", Salary = 4950m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 133, FirstName = "Anne", LastName = "Tyler", Salary = 4650m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 134, FirstName = "Donna", LastName = "Haraway", Salary = 4700m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 135, FirstName = "TaNehisi", LastName = "Coates", Salary = 4800m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 136, FirstName = "Roxane", LastName = "Gay", Salary = 4600m, DepartmentId = 3, ManagerId = 9 },
    new Employee { Id = 137, FirstName = "Walter", LastName = "Mosley", Salary = 5000m, DepartmentId = 3, ManagerId = 1 },
    new Employee { Id = 138, FirstName = "Michael", LastName = "Lewis", Salary = 5450m, DepartmentId = 3, ManagerId = 5 },
    new Employee { Id = 139, FirstName = "Nassim", LastName = "Taleb", Salary = 5400m, DepartmentId = 3, ManagerId = 7 },
    new Employee { Id = 140, FirstName = "Steven", LastName = "Levitt", Salary = 5350m, DepartmentId = 3, ManagerId = 9 },

    // Marketing (Id 141–165)
    new Employee { Id = 141, FirstName = "Clint", LastName = "Eastwood", Salary = 5600m, DepartmentId = 4, ManagerId = 7 },
    new Employee { Id = 142, FirstName = "Quentin", LastName = "Tarantino", Salary = 5500m, DepartmentId = 4, ManagerId = 1 },
    new Employee { Id = 143, FirstName = "Christopher", LastName = "Nolan", Salary = 5700m, DepartmentId = 4, ManagerId = 5 },
    new Employee { Id = 144, FirstName = "Martin", LastName = "Scorsese", Salary = 5800m, DepartmentId = 4, ManagerId = 9 },
    new Employee { Id = 145, FirstName = "Spike", LastName = "Lee", Salary = 5200m, DepartmentId = 4, ManagerId = 7 },
    new Employee { Id = 146, FirstName = "Greta", LastName = "Gerwig", Salary = 5150m, DepartmentId = 4, ManagerId = 1 },
    new Employee { Id = 147, FirstName = "Ava", LastName = "DuVernay", Salary = 5100m, DepartmentId = 4, ManagerId = 5 },
    new Employee { Id = 148, FirstName = "Sofia", LastName = "Coppola", Salary = 5050m, DepartmentId = 4, ManagerId = 9 },
    new Employee { Id = 149, FirstName = "Wes", LastName = "Anderson", Salary = 5000m, DepartmentId = 4, ManagerId = 7 },
    new Employee { Id = 150, FirstName = "David", LastName = "Fincher", Salary = 5450m, DepartmentId = 4, ManagerId = 1 },
    new Employee { Id = 151, FirstName = "Ron", LastName = "Howard", Salary = 5200m, DepartmentId = 4, ManagerId = 5 },
    new Employee { Id = 152, FirstName = "James", LastName = "Cameron", Salary = 5900m, DepartmentId = 4, ManagerId = 9 },
    new Employee { Id = 153, FirstName = "JJ", LastName = "Abrams", Salary = 5300m, DepartmentId = 4, ManagerId = 7 },
    new Employee { Id = 154, FirstName = "Ridley", LastName = "Scott", Salary = 5550m, DepartmentId = 4, ManagerId = 1 },
    new Employee { Id = 155, FirstName = "Denis", LastName = "Villeneuve", Salary = 5600m, DepartmentId = 4, ManagerId = 5 },
    new Employee { Id = 156, FirstName = "Jordan", LastName = "Peele", Salary = 5200m, DepartmentId = 4, ManagerId = 9 },
    new Employee { Id = 157, FirstName = "Taika", LastName = "Waititi", Salary = 5150m, DepartmentId = 4, ManagerId = 7 },
    new Employee { Id = 158, FirstName = "Patty", LastName = "Jenkins", Salary = 5100m, DepartmentId = 4, ManagerId = 1 },
    new Employee { Id = 159, FirstName = "Catherine", LastName = "Hardwicke", Salary = 4850m, DepartmentId = 4, ManagerId = 5 },
    new Employee { Id = 160, FirstName = "Ryan", LastName = "Coogler", Salary = 5250m, DepartmentId = 4, ManagerId = 9 },
    new Employee { Id = 161, FirstName = "Jon", LastName = "Favreau", Salary = 5350m, DepartmentId = 4, ManagerId = 7 },
    new Employee { Id = 162, FirstName = "Barry", LastName = "Jenkins", Salary = 5200m, DepartmentId = 4, ManagerId = 1 },
    new Employee { Id = 163, FirstName = "Seth", LastName = "MacFarlane", Salary = 5000m, DepartmentId = 4, ManagerId = 5 },
    new Employee { Id = 164, FirstName = "Noah", LastName = "Baumbach", Salary = 5050m, DepartmentId = 4, ManagerId = 9 },
    new Employee { Id = 165, FirstName = "David", LastName = "Lynch", Salary = 5400m, DepartmentId = 4, ManagerId = 7 },

    // Sales (Id 166–200)
    new Employee { Id = 166, FirstName = "Stephen", LastName = "King", Salary = 5800m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 167, FirstName = "Mark", LastName = "Twain", Salary = 5600m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 168, FirstName = "Edgar", LastName = "Allan", Salary = 5400m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 169, FirstName = "Walt", LastName = "Whitman", Salary = 5200m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 170, FirstName = "Herman", LastName = "Melville", Salary = 5100m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 171, FirstName = "Nathaniel", LastName = "Hawthorne", Salary = 5050m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 172, FirstName = "Sylvia", LastName = "Plath", Salary = 4750m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 173, FirstName = "Emily", LastName = "Dickinson", Salary = 4800m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 174, FirstName = "Louisa", LastName = "Alcott", Salary = 4650m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 175, FirstName = "Harper", LastName = "Lee", Salary = 4900m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 176, FirstName = "Jack", LastName = "Kerouac", Salary = 5000m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 177, FirstName = "Allen", LastName = "Ginsberg", Salary = 4850m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 178, FirstName = "Tracy", LastName = "McGrady", Salary = 5200m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 179, FirstName = "Reggie", LastName = "Miller", Salary = 5150m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 180, FirstName = "Patrick", LastName = "Ewing", Salary = 5250m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 181, FirstName = "Karl", LastName = "Malone", Salary = 5300m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 182, FirstName = "John", LastName = "Stockton", Salary = 5200m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 183, FirstName = "Hakeem", LastName = "Olajuwon", Salary = 5450m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 184, FirstName = "Tim", LastName = "Duncan", Salary = 5550m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 185, FirstName = "Dirk", LastName = "Nowitzki", Salary = 5400m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 186, FirstName = "Dwyane", LastName = "Wade", Salary = 5450m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 187, FirstName = "Chris", LastName = "Paul", Salary = 5350m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 188, FirstName = "Klay", LastName = "Thompson", Salary = 5400m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 189, FirstName = "Draymond", LastName = "Green", Salary = 5200m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 190, FirstName = "Devin", LastName = "Booker", Salary = 5150m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 191, FirstName = "Jayson", LastName = "Tatum", Salary = 5350m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 192, FirstName = "Jaylen", LastName = "Brown", Salary = 5200m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 193, FirstName = "Nikola", LastName = "Jokic", Salary = 5900m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 194, FirstName = "Luka", LastName = "Doncic", Salary = 5850m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 195, FirstName = "Jimmy", LastName = "Butler", Salary = 5450m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 196, FirstName = "Kyrie", LastName = "Irving", Salary = 5550m, DepartmentId = 5, ManagerId = 9 },
    new Employee { Id = 197, FirstName = "Paul", LastName = "George", Salary = 5400m, DepartmentId = 5, ManagerId = 5 },
    new Employee { Id = 198, FirstName = "Kemba", LastName = "Walker", Salary = 5100m, DepartmentId = 5, ManagerId = 7 },
    new Employee { Id = 199, FirstName = "Zion", LastName = "Williamson", Salary = 5600m, DepartmentId = 5, ManagerId = 1 },
    new Employee { Id = 200, FirstName = "Ja", LastName = "Morant", Salary = 5450m, DepartmentId = 5, ManagerId = 9 }

            );

            b.Entity<Cat>().HasData(
                    new Cat { Id = 1, Name = "Garfield", Age = 6 },
                    new Cat { Id = 2, Name = "Tom", Age = 5 },
                    new Cat { Id = 3, Name = "Sylvester", Age = 7 },
                    new Cat { Id = 4, Name = "Cheshire", Age = 8 },
                    new Cat { Id = 5, Name = "Simba", Age = 4 },
                    new Cat { Id = 6, Name = "Nala", Age = 3 },
                    new Cat { Id = 7, Name = "Puss in Boots", Age = 5 },
                    new Cat { Id = 8, Name = "Felix", Age = 9 },
                    new Cat { Id = 9, Name = "Salem", Age = 10 },
                    new Cat { Id = 10, Name = "Snowball", Age = 2 }
                );

        }
    }
}
