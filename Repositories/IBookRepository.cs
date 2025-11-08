using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface IBookRepository
    {
        Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default);

        Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<int> CreateAsync(BookCreateDto dto, CancellationToken cancellationToken = default);

        Task UpdateAsync(Book entity, BookUpdateDto dto, CancellationToken cancellationToken = default);

        Task DeleteAsync(Book entity, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<BookDataDto> Items, int Total)> GetPageAsync(
            int page, int pageSize, string sort, string order, CancellationToken cancellationToken = default);
    }
}
