using FluentValidation;

namespace UserService.Domain
{
    public class AddOrUpdateUserValidation : AbstractValidator<User>
    {
        public AddOrUpdateUserValidation()
        {
            RuleFor(p => p.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter the First Name")
                .Length(3, 200).WithMessage("The First Name must have between 2 and 150 characters");

            RuleFor(p => p.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Please enter the Last Name")
                .Length(3, 200).WithMessage("The Last Name must have between 2 and 150 characters");
        }
    }
}