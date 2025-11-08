using back_test_project.DTO;

namespace back_test_project.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken ct = default);

        Task<string?> GetNameByIdAsync(int id, CancellationToken ct = default);
    }
}
