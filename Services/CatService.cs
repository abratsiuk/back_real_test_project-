using back_test_project.Models;

namespace back_test_project.Services
{
    public class CatService : ICatService
    {
        private static readonly List<Cat> _cats = new()
        {
            new() { Id = 1, Name = "Garfield", Age = 6 },
            new() { Id = 2, Name = "Tom", Age = 5 },
            new() { Id = 3, Name = "Sylvester", Age = 7 },
            new() { Id = 4, Name = "Cheshire", Age = 8 },
            new() { Id = 5, Name = "Simba", Age = 4 },
            new() { Id = 6, Name = "Nala", Age = 3 },
            new() { Id = 7, Name = "Puss in Boots", Age = 5 },
            new() { Id = 8, Name = "Felix", Age = 9 },
            new() { Id = 9, Name = "Salem", Age = 10 },
            new() { Id = 10, Name = "Snowball", Age = 2 }
        };

        public async Task<IEnumerable<Cat>> GetAllAsync(CancellationToken ct = default)
        {
            await Task.Delay(1000, ct);
            return _cats;
        }

        public async Task<Cat?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            await Task.Delay(1000, ct);
            return _cats.FirstOrDefault(c => c.Id == id);
        }
    }
}
