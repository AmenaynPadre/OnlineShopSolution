using FluentValidation;
using OnlineShop.Api.DTOs;

namespace OnlineShop.Api.Validators
{
    public class UserRegistrationValidator : AbstractValidator<CreateUpdateUserDto>
    {
        public UserRegistrationValidator()
        {
            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(4).WithMessage("Username must be at least 4 characters.")
                .MaximumLength(20).WithMessage("Username must not exceed 20 characters.")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("Username must contain only letters and numbers.");

            // Password validation
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            // FirstName validation
            RuleFor(x => x.Firstname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("First name must contain only letters.");

            // LastName validation
            RuleFor(x => x.Lastname)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
                .Matches("^[a-zA-Z]+$").WithMessage("Last name must contain only letters.");
        }
    }
}
