using Anresh.Api.Controllers.Requests.Employee;
using FluentValidation;

namespace Anresh.Api.Controllers.Requests.User
{
    public class EmailConfirmRequest
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }

    public class EmailConfirmRequestValidator : AbstractValidator<EmailConfirmRequest>
    {
        public EmailConfirmRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token not found");

            RuleFor(x => x.Password)
                .Must(CustomValidators.Password).WithMessage("The password is too simple")
                .MinimumLength(6).WithMessage("The minimum password length is 6 characters")
                .MaximumLength(20).WithMessage("The maximum password length is 20 characters");
        }
    }
}
