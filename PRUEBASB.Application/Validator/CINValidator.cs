using FluentValidation;
using System.Text.RegularExpressions;

namespace PRUEBASB.Application.Validator
{
    public class CINValidator : AbstractValidator<string>
    {
        public CINValidator() {
            var regex = new Regex(@"^\d+$");
            RuleFor(x => x).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("CIN is required,")
                .Matches(regex).WithMessage("CIN must be a valid positive integer.")
                .MinimumLength(11).WithMessage("CIN minimum 11 digit.")
                .MaximumLength(14).WithMessage("CIN maximum 14 digit.");
        }
    }
}
