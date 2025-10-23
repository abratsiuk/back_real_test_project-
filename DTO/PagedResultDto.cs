namespace back_test_project.DTO
{
    public sealed class PagedResultDto<T>
    {
        public IReadOnlyList<T> Data { get; set; } = Array.Empty<T>();
        public int TotalCount { get; set; }
    }
}
