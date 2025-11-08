using back_test_project.DTO;
using FluentValidation;

namespace back_test_project.Validation.Books
{
    public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
    {
        public BookCreateDtoValidator()
        {
            RuleFor(x => x.Title)
                            .NotEmpty().WithMessage("Title is required.")
                            .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.");

            RuleFor(x => x.Authors)
                .NotEmpty().WithMessage("Authors is required.")
                .MaximumLength(400).WithMessage("Authors cannot exceed 400 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(4000).WithMessage("Description cannot exceed 4000 characters.")
                .When(x => x.Description != null);

            int maxYear = DateTime.UtcNow.Year + 1;

            RuleFor(x => x.PublicationYear)
                .InclusiveBetween(1400, maxYear)
                .WithMessage($"Publication year must be between 1400 and {maxYear}.");
        }
    }
}
