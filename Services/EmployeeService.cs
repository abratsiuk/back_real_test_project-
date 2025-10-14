using back_test_project.DTO;
using back_test_project.Repositories;

namespace back_test_project.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        public EmployeeService(IEmployeeRepository repo) => _repo = repo;

        public Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken ct = default)
            => _repo.GetAllDataAsync(ct);

        public Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken ct = default)
            => _repo.GetOptionsAsync(ct);

        public Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default)
            => _repo.GetReadonlyByIdAsync(id, ct);

        public async Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken ct = default)
        {
            return await _repo.CreateAsync(dto, ct);
        }

        public async Task UpdateAsync(int id, EmployeeUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.ManagerId.HasValue && dto.ManagerId.Value == id)
                throw new InvalidOperationException("Employee cannot be his own manager.");

            var entity = await _repo.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Employee not found.");

            await _repo.UpdateAsync(entity, dto, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Employee not found.");

            var hasSubs = await _repo.HasSubordinatesAsync(id, ct);
            if (hasSubs)
                throw new InvalidOperationException("This employee is a manager and cannot be deleted.");

            await _repo.DeleteAsync(entity, ct);
        }
    }
}
