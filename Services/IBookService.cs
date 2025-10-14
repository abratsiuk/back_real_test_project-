// Services/IBookService.cs
using back_test_project.DTO;

public interface IBookService
{
    Task<(IReadOnlyList<BookListItemDto> Items, int Total)> GetPageAsync(int page, int size, string? q);
    //Task<BookDetailsDto?> GetByIdAsync(int id);
    Task<BookDetailsDto?> GetByIdReadOnlyAsync(int id);
    Task<int> CreateAsync(CreateBookDto dto);
    Task UpdateAsync(int id, UpdateBookDto dto);
    Task DeleteAsync(int id);
}
