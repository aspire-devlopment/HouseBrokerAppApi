using FluentValidation;
using HouseBrokerApp.Contracts.Requests;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Full name is required.");

        RuleFor(x => x.UserType)
            .Must(x => x == "Broker" || x == "Seeker")
            .WithMessage("UserType must be 'Broker' or 'Seeker'.");

        RuleFor(x => x.PhoneNumber)
       .NotEmpty().WithMessage("Phone number is required.")
        .Matches(@"^\+?\d{10,14}$").WithMessage("Phone number is invalid. It must be between 10 to 14 digits and can start with '+'.");

    }
}
