using Anresh.Api.Controllers.Requests.Employee;
using FluentValidation;

namespace Anresh.Api.Controllers.Requests.User
{
    public class RegisterUserRequest
    {
        public int EmployeeId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Enter employee Id");

            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Enter email")
                .EmailAddress().WithMessage("Check the spelling of the email");

            RuleFor(x => x.Password)
                .Must(CustomValidators.Password).WithMessage("The password is too simple")
                .MinimumLength(6).WithMessage("The minimum password length is 6 characters")
                .MaximumLength(20).WithMessage("The maximum password length is 20 characters");
        }
    }
}
