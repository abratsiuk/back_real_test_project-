using back_test_project.Data;
using back_test_project.DTO;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _db;
        public EmployeeRepository(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Employees
                .Select(e => new EmployeeDataDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DepartmentName = e.Department.DepartmentName,
                    ManagerFullName = e.Manager != null
                        ? (e.Manager.FirstName + " " + e.Manager.LastName)
                        : null,
                    Salary = e.Salary
                })
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Employees
                .Select(e => new EmployeeOptionDto
                {
                    Id = e.Id,
                    FullName = e.FirstName + " " + e.LastName
                })
                .OrderBy(x => x.FullName)
                .ToListAsync(cancellationToken);
        }

        public async Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Employees
                .Where(e => e.Id == id)
                .Select(e => new EmployeeReadDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    ManagerId = e.ManagerId
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        //With tracking!
        public Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => _db.Employees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);


        public async Task<int> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId,
                ManagerId = dto.ManagerId
            };

            _db.Employees.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task UpdateAsync(Employee entity, EmployeeUpdateDto dto, CancellationToken cancellationToken = default)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Salary = dto.Salary;
            entity.DepartmentId = dto.DepartmentId;
            entity.ManagerId = dto.ManagerId;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Employee entity, CancellationToken cancellationToken = default)
        {
            _db.Employees.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> HasSubordinatesAsync(int managerId, CancellationToken cancellationToken = default)
            => _db.Employees.AnyAsync(e => e.ManagerId == managerId, cancellationToken);

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return _db.Employees.AnyAsync(e => e.Id == id, cancellationToken);
        }

        public async Task<(IReadOnlyList<EmployeeDataDto> Items, int Total)> GetPageAsync(int page, int pageSize, string sort, string order, CancellationToken cancellationToken = default)
        {
            var baseQuery = _db.Employees
              .Select(e => new EmployeeDataDto
              {
                  Id = e.Id,
                  FirstName = e.FirstName,
                  LastName = e.LastName,
                  DepartmentName = e.Department.DepartmentName,
                  ManagerFullName = e.Manager != null
                    ? (e.Manager.FirstName + " " + e.Manager.LastName)
                    : null,
                  Salary = e.Salary
              })
            .AsQueryable();

            var total = await baseQuery.CountAsync(cancellationToken);

            var sortKey = (sort ?? "lastName").Trim().ToLowerInvariant();
            var isAsc = string.Equals(order, "asc", StringComparison.OrdinalIgnoreCase);

            IOrderedQueryable<EmployeeDataDto> ordered = sortKey switch
            {
                "firstname" or "firstName" => isAsc ? baseQuery.OrderBy(x => x.FirstName) : baseQuery.OrderByDescending(x => x.FirstName),
                "lastname" or "lastName" => isAsc ? baseQuery.OrderBy(x => x.LastName) : baseQuery.OrderByDescending(x => x.LastName),
                "departmentname" => isAsc ? baseQuery.OrderBy(x => x.DepartmentName) : baseQuery.OrderByDescending(x => x.DepartmentName),
                "managerfullname" => isAsc ? baseQuery.OrderBy(x => x.ManagerFullName) : baseQuery.OrderByDescending(x => x.ManagerFullName),
                "salary" => isAsc ? baseQuery.OrderBy(x => x.Salary) : baseQuery.OrderByDescending(x => x.Salary),
                _ => isAsc
                    ? baseQuery.OrderBy(x => x.LastName).ThenBy(x => x.FirstName)
                    : baseQuery.OrderByDescending(x => x.LastName).ThenByDescending(x => x.FirstName)
            };

            var skip = Math.Max(0, page) * Math.Max(1, pageSize);


            List<EmployeeDataDto> items;
            if (total == 0)
            {
                items = new List<EmployeeDataDto>();
            }
            else
            {
                items = await ordered
                        .Skip(skip)
                        .Take(Math.Max(1, pageSize))
                        .ToListAsync(cancellationToken);
            }

            return (items, total);
        }
    }
}
