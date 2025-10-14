using back_test_project.DTO;

namespace back_test_project.Services
{
    public interface IDepartmentService
    {
        Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken ct = default);
    }
}
