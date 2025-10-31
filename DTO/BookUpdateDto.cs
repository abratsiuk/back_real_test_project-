namespace back_test_project.DTO
{
    public class BookUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int PublicationYear { get; set; }
    }
}
