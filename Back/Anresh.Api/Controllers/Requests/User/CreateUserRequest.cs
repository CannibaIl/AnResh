using FluentValidation;

namespace Anresh.Api.Controllers.Requests.User
{
    public class CreateUserRequest
    {
        public int EmployeeId {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role {  get; set; }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty()
               .WithMessage("Select employee");

            RuleFor(x => x.Email).NotEmpty()
                .WithMessage("Enter email")
                .EmailAddress().WithMessage("Check the spelling of the email");

            RuleFor(x => x.Role).NotEmpty().WithMessage("Сhoose a role");

            RuleFor(x => x.Password)
                .Must(CustomValidators.Password).WithMessage("The password is too simple")
                .MinimumLength(6).WithMessage("The minimum password length is 6 characters")
                .MaximumLength(20).WithMessage("The maximum password length is 20 characters");
        }
    }
}
