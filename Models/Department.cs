namespace back_test_project.Models
{
    public sealed class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; } = null!;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
