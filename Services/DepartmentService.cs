using back_test_project.DTO;
using back_test_project.Repositories;

namespace back_test_project.Services
{
    public sealed class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repo;
        public DepartmentService(IDepartmentRepository repo) => _repo = repo;

        public Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken ct = default)
            => _repo.GetAllAsync(ct);
    }
}
