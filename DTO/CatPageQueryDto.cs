namespace back_test_project.DTO
{
    public class CatPageQueryDto
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; } = "name";
        public string Order { get; set; } = "asc";
        public string? Name { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
    }
}
