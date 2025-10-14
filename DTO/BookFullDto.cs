namespace back_test_project.DTO
{
    public class BookFullDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int? PublishedYear { get; set; }
        public string? PublishedPlace { get; set; }
        public string? IsbnPrint { get; set; }
        public string? IsbnEbook { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = "English";
        public bool InStock { get; set; }
        public decimal PriceUsd { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<AuthorDto> Authors { get; set; } = new();
    }

    public class CreateBookFullDto
    {
        public string Title { get; set; } = "";
        public int? PublishedYear { get; set; }
        public string? PublishedPlace { get; set; }
        public string? IsbnPrint { get; set; }
        public string? IsbnEbook { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = "English";
        public bool InStock { get; set; }
        public decimal PriceUsd { get; set; }
        public string? CoverUrl { get; set; }
        public List<int> AuthorIds { get; set; } = new();   // many-to-many
    }
    public class UpdateBookFullDto : CreateBookFullDto { }

}
