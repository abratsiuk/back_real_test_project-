using back_test_project.Data;
using back_test_project.DTO;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;
        public BookRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken ct = default)
        {
            return await _db.Books
                .Select(b => new BookDataDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Authors = b.Authors,
                    PublicationYear = b.PublicationYear
                })
                .OrderBy(x => x.Title)
                .ToListAsync(ct);
        }

        public async Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Books
                .Where(b => b.Id == id)
                .Select(b => new BookReadDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Authors = b.Authors,
                    Description = b.Description,
                    PublicationYear = b.PublicationYear
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => _db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

        public async Task<int> CreateAsync(BookCreateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = new Book
            {
                Title = dto.Title,
                Authors = dto.Authors,
                Description = dto.Description,
                PublicationYear = dto.PublicationYear
            };

            _db.Books.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task UpdateAsync(Book entity, BookUpdateDto dto, CancellationToken cancellationToken = default)
        {
            entity.Title = dto.Title;
            entity.Authors = dto.Authors;
            entity.Description = dto.Description;
            entity.PublicationYear = dto.PublicationYear;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Book entity, CancellationToken cancellationToken = default)
        {
            _db.Books.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<BookDataDto> Items, int Total)> GetPageAsync(
            int page, int pageSize, string sort, string order, CancellationToken cancellationToken = default)
        {
            var baseQuery = _db.Books
                .Select(b => new BookDataDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Authors = b.Authors,
                    PublicationYear = b.PublicationYear
                })
                .AsQueryable();

            var total = await _db.Books.CountAsync(cancellationToken);

            var sortKey = (sort ?? "title").Trim().ToLowerInvariant();
            var isAsc = string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase);

            IOrderedQueryable<BookDataDto> ordered = sortKey switch
            {
                "authors" => isAsc ? baseQuery.OrderBy(x => x.Authors) : baseQuery.OrderByDescending(x => x.Authors),
                "publicationyear" => isAsc ? baseQuery.OrderBy(x => x.PublicationYear) : baseQuery.OrderByDescending(x => x.PublicationYear),
                _ => isAsc ? baseQuery.OrderBy(x => x.Title) : baseQuery.OrderByDescending(x => x.Title)
            };

            var skip = Math.Max(0, page) * Math.Max(1, pageSize);

            var items = total == 0
                ? new List<BookDataDto>()
                : await ordered.Skip(skip).Take(Math.Max(1, pageSize)).ToListAsync(cancellationToken);

            return (items, total);
        }
    }
}
