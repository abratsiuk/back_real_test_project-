using back_test_project.DTO;
using FluentValidation;

namespace back_test_project.Validation.Employees
{
    public class EmployeeCreateDtoValidator : AbstractValidator<EmployeeCreateDto>
    {
        public EmployeeCreateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

            RuleFor(x => x.Salary)
                .NotEmpty().WithMessage("Salary is required.")
                .GreaterThanOrEqualTo(0m).WithMessage("Salary cannot be negative.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department id is required.")
                .GreaterThan(0).WithMessage("Department id must be a positive number.");

            RuleFor(x => x.ManagerId)
                .GreaterThan(0)
                .When(x => x.ManagerId.HasValue)
                .WithMessage("Manager id must be a positive number.");
        }
    }
}
