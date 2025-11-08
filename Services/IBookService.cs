using back_test_project.DTO;

namespace back_test_project.Services
{
    public interface IBookService
    {
        Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken ct = default);

        Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default);

        Task<BookDataDto> CreateAsync(BookCreateDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(int id, BookUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<(IReadOnlyList<BookDataDto> items, int total)> GetPageAsync(int page, int pageSize, string sort, string order, CancellationToken ct);
    }
}
