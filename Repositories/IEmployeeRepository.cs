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

        Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);

        Task UpdateAsync(Employee entity, EmployeeUpdateDto dto, CancellationToken cancellationToken = default);

        Task DeleteAsync(Employee entity, CancellationToken cancellationToken = default);

        Task<bool> HasSubordinatesAsync(int managerId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<EmployeeDataDto> Items, int Total)> GetPageAsync(
                int page, int pageSize, string sort, string order, CancellationToken cancellationToken = default);

    }
}
