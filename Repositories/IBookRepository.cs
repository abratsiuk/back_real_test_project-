using back_test_project.Models;

public interface IBookRepository
{
    Task<(IReadOnlyList<Book> Items, int Total)> GetPageAsync(int page, int size, string? q);
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetByIdReadOnlyAsync(int id);
    Task AddAsync(Book book);                // tracks add
    Task DeleteAsync(Book book);             // tracks remove
    Task<bool> ExistsDuplicateAsync(string title, string? authorsString, string? place, int? year, int? excludingId = null);
    Task<int> SaveChangesAsync();
}
