using back_test_project.DTO;
using back_test_project.Repositories;

namespace back_test_project.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository repo) => _repo = repo;

        public Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken ct = default)
            => _repo.GetAllDataAsync(ct);

        public Task<IReadOnlyList<BookOptionDto>> GetOptionsAsync(CancellationToken ct = default)
            => _repo.GetOptionsAsync(ct);

        public Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken ct = default)
            => _repo.GetReadonlyByIdAsync(id, ct);

        public Task<(IReadOnlyList<BookDataDto> items, int total)> GetPageAsync(
            int page, int pageSize, string sort, string order, CancellationToken ct)
            => _repo.GetPageAsync(page, pageSize, sort, order, ct);

        public async Task<int> CreateAsync(BookCreateDto dto, CancellationToken ct = default)
        {
            return await _repo.CreateAsync(dto, ct);
        }

        public async Task UpdateAsync(int id, BookUpdateDto dto, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Book not found.");

            await _repo.UpdateAsync(entity, dto, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _repo.GetByIdAsync(id, ct)
                         ?? throw new KeyNotFoundException("Book not found.");

            await _repo.DeleteAsync(entity, ct);
        }

        public async Task<BookCanDeleteDto> CanDeleteAsync(int id, CancellationToken ct = default)
        {
            var exists = await _repo.ExistsAsync(id, ct);
            if (!exists) return new BookCanDeleteDto { CanDelete = false, Reason = "Book not found." };

            // No special constraints for books now.
            return new BookCanDeleteDto { CanDelete = true };
        }
    }
}
