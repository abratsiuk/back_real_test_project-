using back_test_project.Data;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

public sealed class BookRepository : IBookRepository
{
    private readonly AppDbContext _db;
    public BookRepository(AppDbContext db) => _db = db;

    // Paged list with optional search (Title/AuthorsString/Isbn)
    public async Task<(IReadOnlyList<Book> Items, int Total)> GetPageAsync(int page, int size, string? q)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = _db.Books.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(b =>
                b.Title.Contains(q) ||
                (b.AuthorsString != null && b.AuthorsString.Contains(q)) ||
                (b.IsbnPrint != null && b.IsbnPrint.Contains(q)) ||
                (b.IsbnEbook != null && b.IsbnEbook.Contains(q)));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return (items, total);
    }

    public Task<Book?> GetByIdAsync(int id) =>
        _db.Books.FirstOrDefaultAsync(b => b.Id == id);
    public Task<Book?> GetByIdReadOnlyAsync(int id) => _db.Books
    .AsNoTracking()
    .FirstOrDefaultAsync(b => b.Id == id);

    public async Task AddAsync(Book book)
    {
        // set timestamps here to keep consistency
        book.CreatedAt = DateTime.UtcNow;
        book.UpdatedAt = DateTime.UtcNow;
        await _db.Books.AddAsync(book);
    }

    public Task DeleteAsync(Book book)
    {
        _db.Books.Remove(book);
        return Task.CompletedTask;
    }

    // Duplicate: Title + AuthorsString + PublishedPlace + PublishedYear (case-insensitive)
    public Task<bool> ExistsDuplicateAsync(string title, string? authorsString, string? place, int? year, int? excludingId = null)
    {
        title = title.Trim();
        authorsString = authorsString?.Trim();
        place = place?.Trim();

        var q = _db.Books.AsNoTracking().Where(b =>
            b.Title.ToLower() == title.ToLower() &&
            (b.AuthorsString ?? "").ToLower() == (authorsString ?? "").ToLower() &&
            (b.PublishedPlace ?? "").ToLower() == (place ?? "").ToLower() &&
            b.PublishedYear == year
        );

        if (excludingId.HasValue)
            q = q.Where(b => b.Id != excludingId.Value);

        return q.AnyAsync();
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
