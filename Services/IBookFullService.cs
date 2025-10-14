using back_test_project.DTO;

namespace back_test_project.Services
{
    public interface IBookFullService
    {
        Task<(IReadOnlyList<BookFullDto> Items, int Total)> GetPageAsync(int page, int size, string? q);
        Task<BookFullDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateBookFullDto dto);
        Task UpdateAsync(int id, UpdateBookFullDto dto);
        Task DeleteAsync(int id);
    }

}
