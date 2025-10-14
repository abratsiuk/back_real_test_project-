namespace back_test_project.DTO
{
    public class EmployeeDataDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string? ManagerFullName { get; set; }
        public decimal Salary { get; set; }
    }
}
