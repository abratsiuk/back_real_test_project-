using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default);

        Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken cancellationToken = default);

        Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task CreateAsync(Employee entity, CancellationToken cancellationToken = default);

        void Remove(Employee entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<bool> HasSubordinatesAsync(int managerId, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<EmployeeDataDto> Items, int Total)> GetPageAsync(
            EmployeePageQueryDto query,
            CancellationToken cancellationToken = default);

    }
}
