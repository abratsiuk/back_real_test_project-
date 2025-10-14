using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface IEmployeeRepository
    {
        // Table data
        Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken ct = default);

        // Options list (id + full name)
        Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken ct = default);

        // Read single (for returning one) - no AsNoTracking (as requested)
        Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default);

        // Tracked entity for update/delete
        Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default);

        // Create / Update / Delete
        Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken ct = default);
        Task UpdateAsync(Employee entity, EmployeeUpdateDto dto, CancellationToken ct = default);
        Task DeleteAsync(Employee entity, CancellationToken ct = default);

        // Helpers
        Task<bool> HasSubordinatesAsync(int managerId, CancellationToken ct = default);
    }
}
