using FluentValidation;
using UserService.Domain.Requests;

namespace UserService.Domain.Validators
{
    public class AddUserRequestValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserRequestValidator()
        {
            RuleFor(p => p.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter the First Name")
                .Length(3, 200).WithMessage("The First Name must have between 2 and 150 characters");

            RuleFor(p => p.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter the Last Name")
                .Length(3, 200).WithMessage("The Last Name must have between 2 and 150 characters");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Please enter the E-mail")
                .EmailAddress().WithMessage("Please inform a valid e-mail address");

            RuleFor(p => p.Address).SetValidator(new AddressValidator());
        }
    }
}