using back_test_project.DTO;

namespace back_test_project.Services
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken ct = default);
        Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken ct = default);
        Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default);

        Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken ct = default);
        Task UpdateAsync(int id, EmployeeUpdateDto dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<EmployeeCanDeleteDto> CanDeleteAsync(int id, CancellationToken ct = default);
    }
}
