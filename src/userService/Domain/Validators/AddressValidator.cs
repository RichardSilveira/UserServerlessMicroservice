using FluentValidation;
using UserService.Domain.Requests;

namespace UserService.Domain
{
    public class AddressValidator : AbstractValidator<AddressRequest>
    {
        public AddressValidator()
        {
            RuleFor(p => p.Country)
                .NotEmpty().WithMessage("Please inform the Country");
        }
    }
}