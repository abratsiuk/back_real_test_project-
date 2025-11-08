using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface ICatRepository
    {
        Task<IEnumerable<CatDataDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CatDataDto?> GetReadOnlyByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Cat?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Cat entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(Cat entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(Cat entity, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<CatDataDto> Items, int Total)> GetPageAsync(
            CatPageQueryDto query,
            CancellationToken cancellationToken = default);

    }
}
