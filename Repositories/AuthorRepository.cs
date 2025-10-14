using back_test_project.Data;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

public sealed class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _db;
    public AuthorRepository(AppDbContext db) => _db = db;

    public async Task<(IReadOnlyList<Author> Items, int Total)> GetPageAsync(int page, int size, string? q)
    {
        if (page < 1) page = 1; if (size < 1) size = 20;
        var query = _db.Authors.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(a => a.FullName.Contains(q.Trim()));
        var total = await query.CountAsync();
        var items = await query.OrderBy(a => a.FullName).Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        return (items, total);
    }

    public async Task<IReadOnlyList<Author>> SearchAsync(string? q, int take = 20)
    {
        var query = _db.Authors.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(a => a.FullName.Contains(q.Trim()));
        return await query.OrderBy(a => a.FullName).Take(take).AsNoTracking().ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id, bool tracked = false)
        => tracked
           ? await _db.Authors.FirstOrDefaultAsync(a => a.Id == id)
           : await _db.Authors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);

    public Task<bool> ExistsByNameAsync(string fullName, int? excludingId = null)
    {
        var name = fullName.Trim();
        var q = _db.Authors.AsNoTracking().Where(a => a.FullName == name);
        if (excludingId.HasValue) q = q.Where(a => a.Id != excludingId.Value);
        return q.AnyAsync();
    }

    public Task AddAsync(Author entity) => _db.Authors.AddAsync(entity).AsTask();

    public Task UpdateAsync(Author entity)
    {
        _db.Attach(entity);
        _db.Entry(entity).Property(x => x.FullName).IsModified = true;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        _db.Authors.Remove(new Author { Id = id });
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
