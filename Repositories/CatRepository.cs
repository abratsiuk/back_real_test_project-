using back_test_project.Data;
using back_test_project.DTO;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Repositories
{
    public class CatRepository : ICatRepository
    {
        private readonly AppDbContext _db;

        public CatRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CatDataDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Cats
            .Select(c => new CatDataDto { Id = c.Id, Name = c.Name, Age = c.Age })
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
        }

        public async Task<CatDataDto?> GetReadOnlyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Cats
                .Where(c => c.Id == id)
                .Select(c => new CatDataDto { Id = c.Id, Name = c.Name, Age = c.Age })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Cat?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Cats
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task CreateAsync(Cat entity, CancellationToken cancellationToken = default)
        {
            await _db.Cats.AddAsync(entity, cancellationToken);
        }

        public void Remove(Cat entity)
        {
            _db.Cats.Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<CatDataDto> Items, int Total)> GetPageAsync(
            CatPageQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var baseQuery = _db.Cats
                .Select(c => new CatDataDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Age = c.Age
                });
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                var pattern = query.Name.Trim().ToLower();
                baseQuery = baseQuery.Where(c => c.Name.ToLower().Contains(pattern));
            }
            if (query.MinAge.HasValue)
                baseQuery = baseQuery.Where(c => c.Age >= query.MinAge.Value);

            if (query.MaxAge.HasValue)
                baseQuery = baseQuery.Where(c => c.Age <= query.MaxAge.Value);


            var total = await baseQuery.CountAsync(cancellationToken);

            var safePage = query.Page < 0 ? 0 : query.Page;
            var safePageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            var sortKey = (query.Sort ?? "name").Trim().ToLowerInvariant();
            var isAsc = string.Equals(query.Order, "asc", StringComparison.OrdinalIgnoreCase);


            IOrderedQueryable<CatDataDto> ordered = sortKey switch
            {
                "id" => isAsc
                    ? baseQuery.OrderBy(x => x.Id)
                    : baseQuery.OrderByDescending(x => x.Id),

                "age" => isAsc
                    ? baseQuery.OrderBy(x => x.Age)
                    : baseQuery.OrderByDescending(x => x.Age),

                "name" or _ => isAsc
                    ? baseQuery.OrderBy(x => x.Name)
                    : baseQuery.OrderByDescending(x => x.Name)
            };

            if (total == 0)
            {
                return (Array.Empty<CatDataDto>(), 0);
            }

            var items = await ordered
                .Skip(safePage * safePageSize)
                .Take(safePageSize)
                .ToListAsync(cancellationToken);

            return (items, total);
        }
    }
}
