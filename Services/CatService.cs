using back_test_project.DTO;
using back_test_project.Models;
using back_test_project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Services
{
    public class CatService : ICatService
    {
        private readonly ICatRepository _repository;
        private readonly ILogger<CatService> _logger;

        public CatService(ICatRepository repository, ILogger<CatService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<CatDataDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetAllAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all cats.");
                throw;
            }
        }

        public async Task<CatDataDto?> GetReadOnlyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetReadOnlyByIdAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting cat by id {Id}", id);
                throw;
            }
        }

        public async Task<int> CreateAsync(CatCreateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "CatCreateDto cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Cat name cannot be null or empty.", nameof(dto.Name));
            }
            if (dto.Name.Trim().Length > 50)
            {
                throw new ArgumentException("Cat name cannot exceed 50 characters.", nameof(dto.Name));
            }
            if (dto.Age < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.Age), "Cat age cannot be negative.");
            }
            if (dto.Age > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.Age), "Cat age cannot be greater than 100.");
            }

            var entity = new Cat { Name = dto.Name.Trim(), Age = dto.Age };

            try
            {
                return await _repository.AddAsync(entity, cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error creating new Cat with Name {Name}", dto.Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating new Cat with Name {Name}", dto.Name);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null) { return false; }
            try
            {
                await _repository.DeleteAsync(entity, cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error deleting Cat with Id {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting Cat with Id {Id}", id);
                throw;
            }
            return true;
        }

        public async Task<bool> UpdateAsync(int id, CatUpdateDto dto, CancellationToken cancellationToken = default)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "CatUpdateDto cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Cat name cannot be null or empty.", nameof(dto.Name));
            }
            if (dto.Name.Trim().Length > 50)
            {
                throw new ArgumentException("Cat name cannot exceed 50 characters.", nameof(dto.Name));
            }
            if (dto.Age < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.Age), "Cat age cannot be negative.");
            }
            if (dto.Age > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(dto.Age), "Cat age cannot be greater than 100.");
            }

            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Cat with Id {Id} not found for update.", id);
                return false;
            }

            entity.Name = dto.Name.Trim();
            entity.Age = dto.Age;

            try
            {
                await _repository.UpdateAsync(entity, cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error updating Cat with Id {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating Cat with Id {Id}", id);
                throw;
            }

            return true;
        }


        public async Task<(IReadOnlyList<CatDataDto> Items, int Total)> GetPageAsync(
            CatPageQueryDto query,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetPageAsync(query, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error while getting cats page. page={Page}, pageSize={PageSize}, sort={Sort}, order={Order}",
                    query.Page, query.PageSize, query.Sort, query.Order);
                throw;
            }
        }

    }
}
