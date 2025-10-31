namespace back_test_project.Models
{
    public sealed class Book
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string Authors { get; set; } = null!;

        // Optional long text
        public string? Description { get; set; }

        public int PublicationYear { get; set; }
    }
}
