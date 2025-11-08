using back_test_project.DTO;
using back_test_project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repositoryEmployee;
        private readonly IDepartmentRepository _repositoryDepartment;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(IEmployeeRepository repositoryEmployee,
            IDepartmentRepository repositoryDepartment,
            ILogger<EmployeeService> logger)
        {
            _repositoryEmployee = repositoryEmployee;
            _repositoryDepartment = repositoryDepartment;
            _logger = logger;
        }

        public async Task<IReadOnlyList<EmployeeDataDto>> GetAllDataAsync(CancellationToken ct = default)
        {
            try
            {
                return await _repositoryEmployee.GetAllDataAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all employee data.");
                throw;
            }
        }

        public async Task<IReadOnlyList<EmployeeOptionDto>> GetOptionsAsync(CancellationToken ct = default)
        {
            try
            {
                return await _repositoryEmployee.GetOptionsAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee options.");
                throw;
            }
        }

        public async Task<EmployeeReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                return await _repositoryEmployee.GetReadonlyByIdAsync(id, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching employee by ID.");
                throw;
            }
        }

        public async Task<EmployeeReadDto> CreateAsync(EmployeeCreateDto dto, CancellationToken ct = default)
        {
            var newId = 0;
            try
            {
                newId = await _repositoryEmployee.CreateAsync(dto, ct);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while creating new employee.");
                throw;
            }
            catch (Exception)
            {
                _logger.LogError("Error creating new employee.");
                throw;
            }

            var result = await _repositoryEmployee.GetReadonlyByIdAsync(newId, ct)
             ?? throw new Exception("Failed to retrieve newly created employee.");
            return result;
        }

        public async Task<bool> UpdateAsync(int id, EmployeeUpdateDto dto, CancellationToken ct = default)
        {
            if (dto.ManagerId.HasValue && dto.ManagerId.Value == id)
                throw new InvalidOperationException("Employee cannot be his own manager.");

            var entity = await _repositoryEmployee.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Employee not found.");

            try
            {
                await _repositoryEmployee.UpdateAsync(entity, dto, ct);
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while updating employee with ID {Id}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repositoryEmployee.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Employee not found.");

            var hasSubs = await _repositoryEmployee.HasSubordinatesAsync(id, ct);
            if (hasSubs)
                throw new InvalidOperationException("This employee is a manager and cannot be deleted.");

            try
            {
                await _repositoryEmployee.DeleteAsync(entity, ct);
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting employee with ID {Id}.", id);
                return false;
            }
            catch (Exception)
            {
                _logger.LogError("Error deleting employee with ID {Id}.", id);
                throw;
            }

        }

        public async Task<EmployeeCanDeleteDto> CanDeleteAsync(int id, CancellationToken ct = default)
        {
            var exists = await _repositoryEmployee.ExistsAsync(id, ct);
            if (!exists) return new EmployeeCanDeleteDto { CanDelete = false, Reason = "Employee not found." };

            var hasSubs = await _repositoryEmployee.HasSubordinatesAsync(id, ct);
            if (hasSubs) return new EmployeeCanDeleteDto { CanDelete = false, Reason = "This employee is a manager and cannot be deleted." };

            return new EmployeeCanDeleteDto { CanDelete = true };
        }

        public async Task<(IReadOnlyList<EmployeeDataDto> items, int total)> GetPageAsync(int page, int pageSize, string sort, string order, CancellationToken ct)
        {
            return await _repositoryEmployee.GetPageAsync(page, pageSize, sort, order, ct);
        }
    }
}
