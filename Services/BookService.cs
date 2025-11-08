using back_test_project.DTO;
using back_test_project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly ILogger<BookService> _logger;
        public BookService(IBookRepository repo, ILogger<BookService> logger)
        {
            _repository = repo;
            _logger = logger;
        }

        public async Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken ct = default)
        {
            try
            {
                return await _repository.GetAllDataAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all book data.");
                throw;
            }
        }

        public async Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                return await _repository.GetReadonlyByIdAsync(id, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching readonly book with ID {Id}.", id);
                throw;
            }
        }

        public async Task<(IReadOnlyList<BookDataDto> items, int total)> GetPageAsync(
            int page, int pageSize, string sort, string order, CancellationToken ct)
        {
            try
            {
                return await _repository.GetPageAsync(page, pageSize, sort, order, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching book page.");
                throw;
            }

        }

        public async Task<BookDataDto> CreateAsync(BookCreateDto dto, CancellationToken ct = default)
        {
            try
            {
                var newId = await _repository.CreateAsync(dto, ct);
                return new BookDataDto
                {
                    Id = newId,
                    Title = dto.Title,
                    Authors = dto.Authors,
                    PublicationYear = dto.PublicationYear
                };
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Database update error while creating a new book.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new book.");
                throw;
            }

        }

        public async Task<bool> UpdateAsync(int id, BookUpdateDto dto, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Book not found.");

            try
            {
                await _repository.UpdateAsync(entity, dto, ct);
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while updating book with ID {Id}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating book with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repository.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Book not found.");

            try
            {
                await _repository.DeleteAsync(entity, ct);
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting book with ID {Id}.", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting book with ID {Id}.", id);
                throw;
            }
        }

    }
}
