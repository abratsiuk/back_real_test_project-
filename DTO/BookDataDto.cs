namespace back_test_project.DTO
{
    public class BookDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Authors { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
    }
}
