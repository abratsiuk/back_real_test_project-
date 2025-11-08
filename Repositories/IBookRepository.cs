using back_test_project.DTO;
using back_test_project.Models;

namespace back_test_project.Repositories
{
    public interface IBookRepository
    {
        Task<IReadOnlyList<BookDataDto>> GetAllDataAsync(CancellationToken cancellationToken = default);

        Task<BookReadDto?> GetReadonlyByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task CreateAsync(Book entity, CancellationToken cancellationToken = default);

        void Remove(Book entity);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<(IReadOnlyList<BookDataDto> Items, int Total)> GetPageAsync(
            BookPageQueryDto query, CancellationToken cancellationToken = default);
        Task<bool> ExistsByTitleAndAuthorsAsync(string title, string authors, CancellationToken cancellationToken = default);
        Task<bool> ExistsAnotherWithSameTitleAndAuthorsAsync(int id, string title, string authors, CancellationToken cancellationToken = default);
    }
}
