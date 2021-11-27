using FluentValidation;

namespace Anresh.Api.Controllers.Requests.Skill
{
    public class CreateSkillRequest
    {
        public string Name { get; set; }
    }
    public class CreateSkillRequestValidator : AbstractValidator<CreateSkillRequest>
    {
        public CreateSkillRequestValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Enter Name")
                 .MaximumLength(20).WithMessage("Max length 20");
        }
    }
}