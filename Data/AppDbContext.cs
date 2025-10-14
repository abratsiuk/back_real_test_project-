using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
        public DbSet<Review> Reviews => Set<Review>();
        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<BookAuthor>().HasKey(x => new { x.BookId, x.AuthorId });
            b.Entity<Book>().HasIndex(x => new { x.Title, x.AuthorsString });
        }
    }
}
