using back_test_project.DTO;
using back_test_project.Models;
using back_test_project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Services
{
    public class EmployeeService(IEmployeeRepository repository,
        ILogger<EmployeeService> logger) : IEmployeeService
    {
        private readonly IEmployeeRepository _repository = repository;
        private readonly ILogger<EmployeeService> _logger = logger;

        public async Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetAllDataAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all employee data.");
                throw;
            }
        }

        public async Task<(IReadOnlyList<EmployeeDataDto> Items, int Total)> GetPageAsync(
            EmployeePageQueryDto query,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetPageAsync(query, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error while getting employees page. page={Page}, pageSize={PageSize}, sort={Sort}, order={Order}",
                    query.Page, query.PageSize, query.Sort, query.Order);
                throw;
            }
        }

        public async Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetOptionsAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee options.");
                throw;
            }
        }

        public async Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetReadonlyByIdAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee by ID.");
                throw;
            }
        }

        public async Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = new Employee
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                DepartmentId = dto.DepartmentId,
                ManagerId = dto.ManagerId,
                Salary = dto.Salary
            };

            try
            {
                await _repository.CreateAsync(entity, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);

                var result = new EmployeeReadDto
                {
                    Id = entity.Id,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    DepartmentId = entity.DepartmentId,
                    ManagerId = entity.ManagerId,
                    Salary = entity.Salary
                };
                return result;
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Database update error while creating new Employee with FirstName {FirstName} LastName {LastName}", dto.FirstName, dto.LastName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating new Employee with FirstName {FirstName} LastName {LastName}", dto.FirstName, dto.LastName);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, EmployeeUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Employee with Id {Id} not found for update.", id);
                return false;
            }

            entity.FirstName = dto.FirstName.Trim();
            entity.LastName = dto.LastName.Trim();
            entity.DepartmentId = dto.DepartmentId;
            entity.ManagerId = dto.ManagerId;
            entity.Salary = dto.Salary;

            try
            {
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error updating Employee with Id {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating Employee with Id {Id}", id);
                throw;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Employee with Id {Id} not found for deletion.", id);
                return false;
            }

            bool hasSubs = await _repository.HasSubordinatesAsync(id, cancellationToken);
            if (hasSubs)
            {
                _logger.LogWarning("Attempted to delete Employee with Id {Id} who is a manager.", id);
                return false;
            }

            try
            {
                _repository.Remove(entity);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error deleting Employee with Id {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting Employee with Id {Id}", id);
                throw;
            }
            return true;

        }

        public async Task<EmployeeCanDeleteDto> CanDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            bool hasSubordinate = await _repository.HasSubordinatesAsync(id, cancellationToken);

            if (hasSubordinate)
            {
                return new EmployeeCanDeleteDto
                {
                    CanDelete = false,
                    Reason = "This employee is a manager who has subordinates."
                };
            }

            return new EmployeeCanDeleteDto { CanDelete = true };
        }


    }
}
