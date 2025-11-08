using back_test_project.Data;
using back_test_project.DTO;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Repositories
{
    public class CatRepository(AppDbContext db) : ICatRepository
    {
        private readonly AppDbContext _db = db;

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
            var whereQuery = _db.Cats.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                string pattern = query.Name.Trim().ToLower();
                whereQuery = whereQuery.Where(c => c.Name.ToLower().Contains(pattern));
            }
            if (query.MinAge.HasValue)
            {
                whereQuery = whereQuery.Where(c => c.Age >= query.MinAge.Value);
            }

            if (query.MaxAge.HasValue) whereQuery = whereQuery.Where(c => c.Age <= query.MaxAge.Value);

            int total = await whereQuery.CountAsync(cancellationToken);
            if (total == 0)
            {
                return (Array.Empty<CatDataDto>(), 0);
            }

            var selectedQuery = whereQuery
                .Select(c => new CatDataDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Age = c.Age
                });

            int safePage = query.Page < 0 ? 0 : query.Page;
            int safePageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            string sortKey = (query.Sort ?? "name").Trim().ToLowerInvariant();
            bool isAsc = string.Equals(query.Order, "asc", StringComparison.OrdinalIgnoreCase);


            var orderedQuery = sortKey switch
            {
                "id" => isAsc
                    ? selectedQuery.OrderBy(x => x.Id)
                    : selectedQuery.OrderByDescending(x => x.Id),

                "age" => isAsc
                    ? selectedQuery.OrderBy(x => x.Age)
                    : selectedQuery.OrderByDescending(x => x.Age),

                "name" or _ => isAsc
                    ? selectedQuery.OrderBy(x => x.Name)
                    : selectedQuery.OrderByDescending(x => x.Name)
            };



            var items = await orderedQuery
                .Skip(safePage * safePageSize)
                .Take(safePageSize)
                .ToListAsync(cancellationToken);

            return (items, total);
        }
    }
}
