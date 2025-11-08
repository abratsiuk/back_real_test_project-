namespace back_test_project.DTO
{
    public class BookPageQueryDto
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; } = "title";
        public string Order { get; set; } = "asc";

        public string? Title { get; set; }
        public string? Authors { get; set; }
        public int? MinYear { get; set; }
        public int? MaxYear { get; set; }
    }
}
