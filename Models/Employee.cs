namespace back_test_project.Models
{
    public sealed class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public decimal Salary { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    }
}
