namespace back_test_project.DTO
{
    public class EmployeePageQueryDto
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string Sort { get; set; } = "lastName";
        public string Order { get; set; } = "asc";

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public int? DepartmentId { get; set; }
        public int? ManagerId { get; set; }
    }
}
