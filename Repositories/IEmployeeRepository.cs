using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken ct = default);

        Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken ct = default);

        Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default);

        Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken ct = default);

        Task UpdateAsync(Employee entity, EmployeeUpdateDto dto, CancellationToken ct = default);

        Task DeleteAsync(Employee entity, CancellationToken ct = default);

        Task<bool> HasSubordinatesAsync(int managerId, CancellationToken ct = default);
    }
}
