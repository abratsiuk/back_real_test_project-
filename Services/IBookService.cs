using back_test_project.DTO;

namespace back_test_project.Services
{
    public interface IBookService
    {
        Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default);

        Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<BookDataDto> CreateAsync(BookCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, BookUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<BookDataDto> Items, int Total)> GetPageAsync(BookPageQueryDto query, CancellationToken cancellationToken = default);
    }
}
