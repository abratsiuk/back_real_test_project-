using back_test_project.DTO;

namespace back_test_project.Services
{
    public interface IEmployeeService
    {
        Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default);


        Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken cancellationToken = default);
        Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, EmployeeUpdateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<EmployeeCanDeleteDto> CanDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<EmployeeDataDto> Items, int Total)> GetPageAsync(EmployeePageQueryDto query, CancellationToken cancellationToken = default);
    }
}
