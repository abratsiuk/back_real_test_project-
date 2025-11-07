using back_test_project.DTO;

namespace back_test_project.Services
{
    public interface ICatService
    {
        Task<IEnumerable<CatDataDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CatDataDto?> GetReadOnlyByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(CatCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, CatUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<CatDataDto> Items, int Total)> GetPageAsync(
            int page,
            int pageSize,
            string sort,
            string order,
            CancellationToken cancellationToken = default);
    }
}
