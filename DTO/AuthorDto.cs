namespace back_test_project.DTO
{
    public class AuthorDto { public int Id { get; set; } public string FullName { get; set; } = ""; }
    public class CreateAuthorDto { public string FullName { get; set; } = ""; }
    public class UpdateAuthorDto : CreateAuthorDto { }

}
