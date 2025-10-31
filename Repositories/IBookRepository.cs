using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface IBookRepository
    {
        Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken ct = default);

        Task<IReadOnlyList<BookOptionDto>> GetOptionsAsync(CancellationToken ct = default);

        Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default);

        Task<Book?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<int> CreateAsync(BookCreateDto dto, CancellationToken ct = default);

        Task UpdateAsync(Book entity, BookUpdateDto dto, CancellationToken ct = default);

        Task DeleteAsync(Book entity, CancellationToken ct = default);

        Task<bool> ExistsAsync(int id, CancellationToken ct);

        Task<(IReadOnlyList<BookDataDto> Items, int Total)> GetPageAsync(
            int page, int pageSize, string sort, string order, CancellationToken ct = default);
    }
}
