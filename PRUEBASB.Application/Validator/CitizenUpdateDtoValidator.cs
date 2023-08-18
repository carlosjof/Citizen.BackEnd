using FluentValidation;
using PRUEBASB.Application.ViewModel;

namespace PRUEBASB.Application.Validator
{
    public class CitizenUpdateDtoValidator : AbstractValidator<CitizenUpdateDto>
    {
        public CitizenUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .Matches("^[A-Za-z\\s]*$").WithMessage("Name must contain only letters.")
                .MinimumLength(2).WithMessage("Name minimum 2 digit");

            RuleFor(x => x.LastName)
                .Matches("^[A-Za-z\\s]*$").WithMessage("LastName must contain only letters.")
                .MinimumLength(2).WithMessage("LastName minimum 2 digit");

            RuleFor(x => x.Age)
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
