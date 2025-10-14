using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Employee> Employees => Set<Employee>();

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
                e.Property(x => x.Salary).HasColumnType("decimal(18,2)");

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

                // prevent to be manager for him self
                e.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Employee_Manager_Not_Self", "[ManagerId] IS NULL OR [ManagerId] <> [Id]");
                });

            });

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
                new Employee { Id = 10, FirstName = "Walt", LastName = "Whitman", Salary = 3050m, DepartmentId = 5, ManagerId = 9 }
            );
        }
    }
}
