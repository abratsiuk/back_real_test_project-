using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface ICatRepository
    {
        Task<IEnumerable<CatDataDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CatDataDto?> GetReadOnlyByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Cat?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task CreateAsync(Cat entity, CancellationToken cancellationToken = default);
        void Remove(Cat entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<CatDataDto> Items, int Total)> GetPageAsync(
            CatPageQueryDto query,
            CancellationToken cancellationToken = default);
    }
}
