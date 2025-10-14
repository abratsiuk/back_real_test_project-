namespace back_test_project.DTO
{
    public class EmployeeReadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public int? ManagerId { get; set; }
    }
}
