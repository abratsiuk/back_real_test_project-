using back_test_project.Data;
using back_test_project.DTO;
using back_test_project.Models;
using back_test_project.Services;
using Microsoft.EntityFrameworkCore;

public sealed class BookFullService : IBookFullService
{
    private readonly AppDbContext _db;
    public BookFullService(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<BookFullDto> Items, int Total)> GetPageAsync(int page, int size, string? q)
    {
        if (page < 1) page = 1; if (size < 1) size = 20;

        var query = _db.Books.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(b => b.Title.Contains(q) || (b.IsbnPrint ?? "").Contains(q) || (b.IsbnEbook ?? "").Contains(q));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderBy(b => b.Title)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(b => new BookFullDto
            {
                Id = b.Id,
                Title = b.Title,
                PublishedYear = b.PublishedYear,
                PublishedPlace = b.PublishedPlace,
                IsbnPrint = b.IsbnPrint,
                IsbnEbook = b.IsbnEbook,
                Description = b.Description,
                Language = b.Language,
                InStock = b.InStock,
                PriceUsd = b.PriceUsd,
                CoverUrl = b.CoverUrl,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                Authors = b.BookAuthors.Select(ba => new AuthorDto { Id = ba.Author.Id, FullName = ba.Author.FullName }).ToList()
            })
            .ToListAsync();

        return (items, total);
    }

    public async Task<BookFullDto?> GetByIdAsync(int id)
    {
        return await _db.Books.AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookFullDto
            {
                Id = b.Id,
                Title = b.Title,
                PublishedYear = b.PublishedYear,
                PublishedPlace = b.PublishedPlace,
                IsbnPrint = b.IsbnPrint,
                IsbnEbook = b.IsbnEbook,
                Description = b.Description,
                Language = b.Language,
                InStock = b.InStock,
                PriceUsd = b.PriceUsd,
                CoverUrl = b.CoverUrl,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt,
                Authors = b.BookAuthors.Select(ba => new AuthorDto { Id = ba.Author.Id, FullName = ba.Author.FullName }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateAsync(CreateBookFullDto dto)
    {
        // duplicate check (Title + authors + place + year)
        var normalizedTitle = dto.Title.Trim().ToLower();
        var authorSet = dto.AuthorIds.Distinct().ToList(); // локальный список

        var exists = await _db.Books.AsNoTracking()
            .Where(b => b.Title.ToLower() == normalizedTitle
                     && b.PublishedYear == dto.PublishedYear
                     && (b.PublishedPlace ?? "") == (dto.PublishedPlace ?? ""))
            // равенство множеств авторов: размер одинаковый И нет автора вне переданного набора
            .Where(b => b.BookAuthors.Count() == authorSet.Count)
            .Where(b => !b.BookAuthors.Any(ba => !authorSet.Contains(ba.AuthorId)))
            .AnyAsync();

        if (exists) throw new InvalidOperationException("Duplicate book candidate.");


        var entity = new Book
        {
            Title = dto.Title.Trim(),
            PublishedYear = dto.PublishedYear,
            PublishedPlace = dto.PublishedPlace?.Trim(),
            IsbnPrint = dto.IsbnPrint?.Trim(),
            IsbnEbook = dto.IsbnEbook?.Trim(),
            Description = dto.Description,
            Language = string.IsNullOrWhiteSpace(dto.Language) ? "English" : dto.Language.Trim(),
            InStock = dto.InStock,
            PriceUsd = dto.PriceUsd,
            CoverUrl = dto.CoverUrl
        };

        // attach authors
        if (dto.AuthorIds.Count > 0)
        {
            var authors = await _db.Authors.Where(a => dto.AuthorIds.Contains(a.Id)).Select(a => a.Id).ToListAsync();
            foreach (var aid in authors)
                entity.BookAuthors.Add(new BookAuthor { AuthorId = aid, Book = entity });
        }

        await _db.Books.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(int id, UpdateBookFullDto dto)
    {
        var b = await _db.Books
            .Include(x => x.BookAuthors)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new KeyNotFoundException("Book not found.");

        b.Title = dto.Title.Trim();
        b.PublishedYear = dto.PublishedYear;
        b.PublishedPlace = dto.PublishedPlace?.Trim();
        b.IsbnPrint = dto.IsbnPrint?.Trim();
        b.IsbnEbook = dto.IsbnEbook?.Trim();
        b.Description = dto.Description;
        b.Language = string.IsNullOrWhiteSpace(dto.Language) ? "English" : dto.Language.Trim();
        b.InStock = dto.InStock;
        b.PriceUsd = dto.PriceUsd;
        b.CoverUrl = dto.CoverUrl;
        b.UpdatedAt = DateTime.UtcNow;

        // sync many-to-many (replace set)
        var incoming = dto.AuthorIds.Distinct().ToHashSet();

        // удалить тех, кого нет в incoming
        foreach (var old in b.BookAuthors.Where(ba => !incoming.Contains(ba.AuthorId)).ToList())
            b.BookAuthors.Remove(old);

        // добавить новых авторов
        var existingIds = b.BookAuthors.Select(ba => ba.AuthorId).ToHashSet();
        foreach (var aid in incoming.Except(existingIds))
            b.BookAuthors.Add(new BookAuthor { AuthorId = aid, BookId = b.Id });


        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var b = await _db.Books.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException("Book not found.");
        _db.Books.Remove(b);
        await _db.SaveChangesAsync();
    }
}
