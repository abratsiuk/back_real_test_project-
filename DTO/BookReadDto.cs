namespace back_test_project.DTO
{
    public class BookReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PublicationYear { get; set; }
    }
}
