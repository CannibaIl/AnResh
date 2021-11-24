using FluentValidation;

namespace Anresh.Api.Controllers.Requests.User
{
    public class ChangeRoleRequest
    {
        public int Id {  get; set; }
        public string Role {  get; set; }
    }

    public class ChangeRoleRequestValidator : AbstractValidator<ChangeRoleRequest>
    {
        public ChangeRoleRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Select user");

            RuleFor(x => x.Role).NotEmpty().WithMessage("Сhoose a role");
        }
    }
}
