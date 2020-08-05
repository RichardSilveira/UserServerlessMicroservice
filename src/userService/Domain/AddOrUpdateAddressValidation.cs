using FluentValidation;

namespace UserService.Domain
{
    public class ChangeAddressValidation : AbstractValidator<Address>
    {
        public ChangeAddressValidation()
        {
            RuleFor(p => p.Country)
                .NotEmpty().WithMessage("Please inform the Country");
        }
    }
}