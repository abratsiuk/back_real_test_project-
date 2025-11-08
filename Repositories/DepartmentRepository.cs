using back_test_project.Data;
using back_test_project.DTO;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Repositories
{
    public sealed class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _db;
        public DepartmentRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync(CancellationToken ct = default)
        {
            return await _db.Departments
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    DepartmentName = d.DepartmentName
                })
                .OrderBy(d => d.DepartmentName)
                .ToListAsync(ct);
        }

        public async Task<string?> GetNameByIdAsync(int id, CancellationToken ct = default)
        {
            return await _db.Departments
                .Where(d => d.Id == id)
                .Select(d => d.DepartmentName)
                .FirstOrDefaultAsync(ct);
        }
    }
}
