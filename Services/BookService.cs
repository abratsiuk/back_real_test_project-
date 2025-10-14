// Services/BookService.cs
using back_test_project.DTO;
using back_test_project.Models;

public sealed class BookService : IBookService
{
    private readonly IBookRepository _repo;
    public BookService(IBookRepository repo) => _repo = repo;

    public async Task<(IReadOnlyList<BookListItemDto> Items, int Total)> GetPageAsync(int page, int size, string? q)
    {
        var (items, total) = await _repo.GetPageAsync(page, size, q);
        // Manual projection to DTO
        var dto = items.Select(b => new BookListItemDto
        {
            Id = b.Id,
            Title = b.Title,
            PublishedYear = b.PublishedYear,
            PublishedPlace = b.PublishedPlace,
            Language = b.Language,
            InStock = b.InStock,
            PriceUsd = b.PriceUsd,
            CoverUrl = b.CoverUrl,
            AuthorsString = b.AuthorsString,
            IsbnPrint = b.IsbnPrint,
            IsbnEbook = b.IsbnEbook,
            Description = b.Description,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt
        }).ToList();
        return (dto, total);
    }

    //public async Task<BookDetailsDto?> GetByIdAsync(int id)
    //{
    //    var b = await _repo.GetByIdAsync(id);
    //    if (b is null) return null;
    //    return new BookDetailsDto
    //    {
    //        Id = b.Id,
    //        Title = b.Title,
    //        PublishedYear = b.PublishedYear,
    //        PublishedPlace = b.PublishedPlace,
    //        Language = b.Language,
    //        InStock = b.InStock,
    //        PriceUsd = b.PriceUsd,
    //        CoverUrl = b.CoverUrl,
    //        AuthorsString = b.AuthorsString,
    //        IsbnPrint = b.IsbnPrint,
    //        IsbnEbook = b.IsbnEbook,
    //        Description = b.Description,
    //        CreatedAt = b.CreatedAt,
    //        UpdatedAt = b.UpdatedAt
    //    };
    //}
    public async Task<BookDetailsDto?> GetByIdReadOnlyAsync(int id)
    {
        var b = await _repo.GetByIdReadOnlyAsync(id);
        if (b is null) return null;
        return new BookDetailsDto
        {
            Id = b.Id,
            Title = b.Title,
            PublishedYear = b.PublishedYear,
            PublishedPlace = b.PublishedPlace,
            Language = b.Language,
            InStock = b.InStock,
            PriceUsd = b.PriceUsd,
            CoverUrl = b.CoverUrl,
            AuthorsString = b.AuthorsString,
            IsbnPrint = b.IsbnPrint,
            IsbnEbook = b.IsbnEbook,
            Description = b.Description,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt
        };
    }

    public async Task<int> CreateAsync(CreateBookDto dto)
    {
        // Business validation: duplicate check
        if (await _repo.ExistsDuplicateAsync(dto.Title, dto.AuthorsString, dto.PublishedPlace, dto.PublishedYear))
            throw new InvalidOperationException("Duplicate book candidate.");

        var b = new Book
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
            CoverUrl = dto.CoverUrl,
            AuthorsString = dto.AuthorsString
        };

        await _repo.AddAsync(b);
        await _repo.SaveChangesAsync();
        return b.Id;
    }

    public async Task UpdateAsync(int id, UpdateBookDto dto)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Book not found.");

        // Apply updates
        existing.Title = dto.Title.Trim();
        existing.PublishedYear = dto.PublishedYear;
        existing.PublishedPlace = dto.PublishedPlace?.Trim();
        existing.IsbnPrint = dto.IsbnPrint?.Trim();
        existing.IsbnEbook = dto.IsbnEbook?.Trim();
        existing.Description = dto.Description;
        existing.Language = string.IsNullOrWhiteSpace(dto.Language) ? "English" : dto.Language.Trim();
        existing.InStock = dto.InStock;
        existing.PriceUsd = dto.PriceUsd;
        existing.CoverUrl = dto.CoverUrl;
        existing.AuthorsString = dto.AuthorsString;
        existing.UpdatedAt = DateTime.UtcNow;

        if (await _repo.ExistsDuplicateAsync(existing.Title, existing.AuthorsString, existing.PublishedPlace, existing.PublishedYear, excludingId: id))
            throw new InvalidOperationException("Duplicate book candidate.");

        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Book not found.");
        await _repo.DeleteAsync(existing);
        await _repo.SaveChangesAsync();
    }
}
