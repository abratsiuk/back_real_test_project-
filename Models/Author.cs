namespace back_test_project.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
