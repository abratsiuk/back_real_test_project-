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

        public async Task<int> AddAsync(Cat entity, CancellationToken cancellationToken = default)
        {
            await _db.Cats.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task DeleteAsync(Cat entity, CancellationToken cancellationToken = default)
        {
            _db.Cats.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }


        public async Task UpdateAsync(Cat entity, CancellationToken cancellationToken = default)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IReadOnlyList<CatDataDto> Items, int Total)> GetPageAsync(
            int page,
            int pageSize,
            string sort,
            string order,
            CancellationToken cancellationToken = default)
        {
            var baseQuery = _db.Cats
                .Select(c => new CatDataDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Age = c.Age
                });

            var total = await baseQuery.CountAsync(cancellationToken);

            var safePage = page < 0 ? 0 : page;
            var safePageSize = pageSize <= 0 ? 10 : pageSize;

            var sortKey = (sort ?? "name").Trim().ToLowerInvariant();
            var isAsc = string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase);

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
