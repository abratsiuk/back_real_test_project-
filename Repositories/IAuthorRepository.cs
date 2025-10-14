using back_test_project.Models;

public interface IAuthorRepository
{
    Task<(IReadOnlyList<Author> Items, int Total)> GetPageAsync(int page, int size, string? q);
    Task<IReadOnlyList<Author>> SearchAsync(string? q, int take = 20);
    Task<Author?> GetByIdAsync(int id, bool tracked = false);
    Task<bool> ExistsByNameAsync(string fullName, int? excludingId = null);
    Task AddAsync(Author entity);
    Task UpdateAsync(Author entity);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync();
}
