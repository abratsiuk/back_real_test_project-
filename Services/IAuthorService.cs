using back_test_project.DTO;

public interface IAuthorService
{
    Task<(IReadOnlyList<AuthorDto> Items, int Total)> GetPageAsync(int page, int size, string? q);
    Task<IReadOnlyList<AuthorDto>> SearchAsync(string? q, int take = 20);
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateAuthorDto dto);
    Task UpdateAsync(int id, UpdateAuthorDto dto);
    Task DeleteAsync(int id);
}
