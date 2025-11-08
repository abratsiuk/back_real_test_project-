using back_test_project.Data;
using back_test_project.DTO;
using back_test_project.Models;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Repositories
{
    public class EmployeeRepository(AppDbContext db) : IEmployeeRepository
    {
        private readonly AppDbContext _db = db;

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
        {
            return _db.Employees.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task CreateAsync(Employee entity, CancellationToken cancellationToken = default)
        {
            await _db.Employees.AddAsync(entity, cancellationToken);
        }

        public void Remove(Employee entity)
        {
            _db.Employees.Remove(entity);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        public Task<bool> HasSubordinatesAsync(int managerId, CancellationToken cancellationToken = default)
        {
            return _db.Employees.AnyAsync(e => e.ManagerId == managerId, cancellationToken);
        }

        public async Task<(IReadOnlyList<EmployeeDataDto> Items, int Total)> GetPageAsync(EmployeePageQueryDto query, CancellationToken cancellationToken = default)
        {
            var whereQuery = _db.Employees.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.FirstName))
            {
                string pattern = query.FirstName.Trim().ToLowerInvariant();
                whereQuery = whereQuery.Where(e => e.FirstName.ToLower().Contains(pattern));
            }
            if (!string.IsNullOrWhiteSpace(query.LastName))
            {
                string pattern = query.LastName.Trim().ToLowerInvariant();
                whereQuery = whereQuery.Where(e => e.LastName.ToLower().Contains(pattern));
            }
            if (query.MinSalary.HasValue)
            {
                whereQuery = whereQuery.Where(e => e.Salary >= query.MinSalary.Value);
            }
            if (query.MaxSalary.HasValue)
            {
                whereQuery = whereQuery.Where(e => e.Salary <= query.MaxSalary.Value);
            }
            if (query.DepartmentId.HasValue)
            {
                whereQuery = whereQuery.Where(e => e.DepartmentId == query.DepartmentId.Value);
            }
            if (query.ManagerId.HasValue)
            {
                whereQuery = whereQuery.Where(e => e.ManagerId == query.ManagerId.Value);
            }

            int total = await whereQuery.CountAsync(cancellationToken);
            if (total == 0)
            {
                return (Array.Empty<EmployeeDataDto>(), 0);
            }

            var selectQuery = whereQuery.Select(e => new EmployeeDataDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                DepartmentName = e.Department.DepartmentName,
                ManagerFullName = e.Manager != null
                    ? (e.Manager.FirstName + " " + e.Manager.LastName)
                    : null,
                Salary = e.Salary
            });

            string sortKey = (query.Sort ?? "lastname").Trim().ToLowerInvariant();
            bool isAsc = string.Equals(query.Order, "asc", StringComparison.OrdinalIgnoreCase);

            var orderedQuery = sortKey switch
            {
                "id" => isAsc
                    ? selectQuery.OrderBy(x => x.Id)
                    : selectQuery.OrderByDescending(x => x.Id),
                "firstname" => isAsc
                    ? selectQuery.OrderBy(x => x.FirstName)
                    : selectQuery.OrderByDescending(x => x.FirstName),
                "lastname" => isAsc
                    ? selectQuery.OrderBy(x => x.LastName)
                    : selectQuery.OrderByDescending(x => x.LastName),
                "departmentname" => isAsc
                    ? selectQuery.OrderBy(x => x.DepartmentName)
                    : selectQuery.OrderByDescending(x => x.DepartmentName),
                "managerfullname" => isAsc
                    ? selectQuery.OrderBy(x => x.ManagerFullName)
                    : selectQuery.OrderByDescending(x => x.ManagerFullName),
                "salary" => isAsc
                    ? selectQuery.OrderBy(x => x.Salary)
                    : selectQuery.OrderByDescending(x => x.Salary),
                _ => isAsc
                    ? selectQuery.OrderBy(x => x.LastName)
                    : selectQuery.OrderByDescending(x => x.LastName)
            };

            int skip = Math.Max(0, query.Page) * Math.Max(1, query.PageSize);

            var items = await orderedQuery
                        .Skip(skip)
                        .Take(Math.Max(1, query.PageSize))
                        .ToListAsync(cancellationToken);
            return (items, total);
        }
    }
}
