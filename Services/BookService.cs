using back_test_project.DTO;
using back_test_project.Models;
using back_test_project.Repositories;
using Microsoft.EntityFrameworkCore;

namespace back_test_project.Services
{
    public class BookService(IBookRepository repo, ILogger<BookService> logger) : IBookService
    {
        private readonly IBookRepository _repository = repo;
        private readonly ILogger<BookService> _logger = logger;

        public async Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetAllDataAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all book data.");
                throw;
            }
        }

        public async Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetReadonlyByIdAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching readonly book with ID {Id}.", id);
                throw;
            }
        }


        public async Task<(IReadOnlyList<BookDataDto> Items, int Total)> GetPageAsync(
        BookPageQueryDto query,
        CancellationToken cancellationToken = default)
        {
            try
            {
                return await _repository.GetPageAsync(query, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error while getting books page. page={Page}, pageSize={PageSize}, sort={Sort}, order={Order}",
                    query.Page, query.PageSize, query.Sort, query.Order);
                throw;
            }
        }

        public async Task<BookDataDto> CreateAsync(BookCreateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = new Book
            {
                Title = dto.Title.Trim(),
                Authors = dto.Authors.Trim(),
                Description = dto.Description?.Trim(),
                PublicationYear = dto.PublicationYear
            };
            try
            {
                await _repository.CreateAsync(entity, cancellationToken);
                await _repository.SaveChangesAsync(cancellationToken);
                var result = new BookDataDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Authors = entity.Authors,
                    PublicationYear = entity.PublicationYear
                };
                return result;
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error creating new Book with Title {Title}", dto.Title);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating new Book with Title {Title}", dto.Title);
                throw;
            }

        }

        public async Task<bool> UpdateAsync(int id, BookUpdateDto dto, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Book with Id {Id} not found for update.", id);
                return false;
            }

            entity.Title = dto.Title.Trim();
            entity.Authors = dto.Authors.Trim();
            entity.Description = dto.Description?.Trim();
            entity.PublicationYear = dto.PublicationYear;

            try
            {
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error updating Book with Id {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating Book with Id {Id}", id);
                throw;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                _logger.LogWarning("Book with Id {Id} not found for deletion.", id);
                return false;
            }

            try
            {
                _repository.Remove(entity);
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Error deleting Book with Id {Id}", id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting Book with Id {Id}", id);
                throw;
            }
            return true;
        }

    }
}
