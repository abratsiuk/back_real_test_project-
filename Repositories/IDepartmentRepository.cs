using back_test_project.DTO;

namespace back_test_project.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<string?> GetNameByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
