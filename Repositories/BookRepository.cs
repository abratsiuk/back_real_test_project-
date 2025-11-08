using back_test_project.Data;
using back_test_project.DTO;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Repositories
{
    public class BookRepository(AppDbContext db) : IBookRepository
    {
        private readonly AppDbContext _db = db;

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
        {
            return _db.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task CreateAsync(Book entity, CancellationToken cancellationToken = default)
        {
            await _db.Books.AddAsync(entity, cancellationToken);
        }

        public void Remove(Book entity)
        {
            _db.Books.Remove(entity);
        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<BookDataDto> Items, int Total)> GetPageAsync(
            BookPageQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var whereQuery = _db.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                string pattern = query.Title.Trim().ToLowerInvariant();
                whereQuery = whereQuery.Where(b => b.Title.ToLowerInvariant().Contains(pattern));
            }
            if (!string.IsNullOrWhiteSpace(query.Authors))
            {
                string pattern = query.Authors.Trim().ToLowerInvariant();
                whereQuery = whereQuery.Where(b => b.Authors.ToLowerInvariant().Contains(pattern));
            }

            if (query.MinYear.HasValue)
            {
                whereQuery = whereQuery.Where(b => b.PublicationYear >= query.MinYear.Value);
            }

            if (query.MaxYear.HasValue)
            {
                whereQuery = whereQuery.Where(b => b.PublicationYear <= query.MaxYear.Value);
            }

            int total = await whereQuery.CountAsync(cancellationToken);
            if (total == 0)
            {
                return (Array.Empty<BookDataDto>(), 0);
            }

            var selectQuery = whereQuery.Select(b => new BookDataDto
            {
                Id = b.Id,
                Title = b.Title,
                Authors = b.Authors,
                PublicationYear = b.PublicationYear
            });

            string sortKey = (query.Sort ?? "title").Trim().ToLowerInvariant();
            bool isAsc = string.Equals(query.Order, "asc", StringComparison.OrdinalIgnoreCase);

            var orderedQuery = sortKey switch
            {
                "id" => isAsc ? selectQuery.OrderBy(x => x.Id) : selectQuery.OrderByDescending(x => x.Id),
                "title" => isAsc ? selectQuery.OrderBy(x => x.Title) : selectQuery.OrderByDescending(x => x.Title),
                "authors" => isAsc ? selectQuery.OrderBy(x => x.Authors) : selectQuery.OrderByDescending(x => x.Authors),
                "publicationyear" => isAsc ? selectQuery.OrderBy(x => x.PublicationYear) : selectQuery.OrderByDescending(x => x.PublicationYear),
                _ => isAsc
                    ? selectQuery.OrderBy(x => x.Title)
                    : selectQuery.OrderByDescending(x => x.Title)
            };

            int skip = Math.Max(0, query.Page) * Math.Max(1, query.PageSize);

            var items = await orderedQuery
                        .Skip(skip)
                        .Take(Math.Max(1, query.PageSize))
                        .ToListAsync(cancellationToken);

            return (items, total);
        }
    }
}
