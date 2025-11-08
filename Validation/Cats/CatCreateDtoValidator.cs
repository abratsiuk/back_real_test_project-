using back_test_project.DTO;
using FluentValidation;

namespace back_test_project.Validation.Cats
{
    public class CatCreateDtoValidator : AbstractValidator<CatCreateDto>
    {
        public CatCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Cat name cannot be null or empty.")
                .MaximumLength(50).WithMessage("Cat name cannot exceed 50 characters.");
            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(0).WithMessage("Cat age cannot be negative.")
                .InclusiveBetween(0, 100)
                .WithMessage("Cat age must be between 0 and 100.");
        }
    }
}
