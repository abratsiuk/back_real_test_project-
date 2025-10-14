using back_test_project.DTO;
using back_test_project.Models;

public sealed class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repo;
    public AuthorService(IAuthorRepository repo) => _repo = repo;

    public async Task<(IReadOnlyList<AuthorDto> Items, int Total)> GetPageAsync(int page, int size, string? q)
    {
        var (items, total) = await _repo.GetPageAsync(page, size, q);
        return (items.Select(a => new AuthorDto { Id = a.Id, FullName = a.FullName }).ToList(), total);
    }

    public async Task<IReadOnlyList<AuthorDto>> SearchAsync(string? q, int take = 20)
        => (await _repo.SearchAsync(q, take)).Select(a => new AuthorDto { Id = a.Id, FullName = a.FullName }).ToList();

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        var a = await _repo.GetByIdAsync(id);
        return a is null ? null : new AuthorDto { Id = a.Id, FullName = a.FullName };
    }

    public async Task<int> CreateAsync(CreateAuthorDto dto)
    {
        var name = dto.FullName.Trim();
        if (await _repo.ExistsByNameAsync(name)) throw new InvalidOperationException("Author already exists.");
        var entity = new Author { FullName = name };
        await _repo.AddAsync(entity);
        await _repo.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(int id, UpdateAuthorDto dto)
    {
        var exists = await _repo.GetByIdAsync(id);
        if (exists is null) throw new KeyNotFoundException("Author not found.");
        var name = dto.FullName.Trim();
        if (await _repo.ExistsByNameAsync(name, excludingId: id)) throw new InvalidOperationException("Author already exists.");
        await _repo.UpdateAsync(new Author { Id = id, FullName = name });
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _repo.GetByIdAsync(id);
        if (exists is null) throw new KeyNotFoundException("Author not found.");
        await _repo.DeleteAsync(id);
        await _repo.SaveChangesAsync();
    }
}
