using FluentValidation;

namespace Anresh.Api.Controllers.Requests.User
{
    public class RestorePasswordRequest
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }

    public class RestorePasswordRequestValidator : AbstractValidator<RestorePasswordRequest>
    {
        public RestorePasswordRequestValidator()
        {
            RuleFor(x => x.Token).NotEmpty()
                .WithMessage("Token is empty");

            RuleFor(x => x.Password)
                .Must(CustomValidators.Password).WithMessage("The password is too simple")
                .MinimumLength(6).WithMessage("The minimum password length is 6 characters")
                .MaximumLength(20).WithMessage("The maximum password length is 20 characters");
        }
    }
}
