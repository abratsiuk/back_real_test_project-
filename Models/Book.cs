using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace back_test_project.Models
{
    public class Book
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

        [Precision(10, 2)]
        public decimal PriceUsd { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(400)]//TEMP: comma-separated authors, e.g. "John Doe, Jane Roe"
        public string? AuthorsString { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
