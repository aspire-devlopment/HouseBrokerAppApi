using FluentValidation;
using HouseBrokerApp.Contracts.Requests;

namespace HouseBrokerApp.API.Validators;

public class CreatePropertyListingRequestValidator : AbstractValidator<CreatePropertyListingRequest>
{
    public CreatePropertyListingRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Location).NotEmpty();
    }
}
