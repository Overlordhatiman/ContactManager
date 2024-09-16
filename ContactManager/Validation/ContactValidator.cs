using ContactManager.Models;
using FluentValidation;

namespace ContactManager.Validation
{

    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.DateOfBirth).NotEmpty().LessThan(DateTime.Today).WithMessage("Invalid Date of Birth");
            RuleFor(c => c.Married).NotNull().WithMessage("Married status is required");
            RuleFor(c => c.Phone).NotEmpty().Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits");
            RuleFor(c => c.Salary).GreaterThan(0).WithMessage("Salary must be greater than zero");
        }
    }
}
