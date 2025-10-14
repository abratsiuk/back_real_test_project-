namespace back_test_project.DTO
{
    // DTO/BookDto.cs
    public class BookListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int? PublishedYear { get; set; }
        public string? PublishedPlace { get; set; }
        public string Language { get; set; } = "English";
        public bool InStock { get; set; }
        public decimal PriceUsd { get; set; }
        public string? CoverUrl { get; set; }
        public string? AuthorsString { get; set; }
        public string? IsbnPrint { get; set; }
        public string? IsbnEbook { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BookDetailsDto : BookListItemDto
    {

    }

    // Create/Update payloads (client → server)
    public class CreateBookDto
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
        public string? AuthorsString { get; set; }
    }

    public class UpdateBookDto : CreateBookDto { }

}
