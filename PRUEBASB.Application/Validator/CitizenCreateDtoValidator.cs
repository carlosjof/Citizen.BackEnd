using FluentValidation;
using PRUEBASB.Application.ViewModel;

namespace PRUEBASB.Application.Validator
{
    public class CitizenCreateDtoValidator : AbstractValidator<CitizenCreateDto>
    {
        public CitizenCreateDtoValidator()
        {
            RuleFor(x => x.CIN)
                .NotEmpty().WithMessage("CIN is required")
                .Matches(@"^\d+$").WithMessage("CIN must be a valid positive integer.")
                .MinimumLength(11).WithMessage("CIN minimum 11 digit.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Matches("^[A-Za-z\\s]*$").WithMessage("Name must contain only letters.")
                .MinimumLength(2).WithMessage("Name minimum 2 digit");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .Matches("^[A-Za-z\\s]*$").WithMessage("LastName must contain only letters.")
                .MinimumLength(2).WithMessage("LastName minimum 2 digit");

            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required")
                .InclusiveBetween(0, 200).WithMessage("Age must be between 0 and 120.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(BeValidGender).WithMessage("Gender must be 'M' or 'F'.");
        }

        private bool BeValidGender(string value)
        {
            return value == "M" || value == "F";
        }
    }
}
