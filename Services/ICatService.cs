using back_test_project.Models;

namespace back_test_project.Services
{
    public interface ICatService
    {
        Task<IEnumerable<Cat>> GetAllAsync(CancellationToken ct = default);
        Task<Cat?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}
