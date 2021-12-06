using FluentValidation;

namespace Anresh.Api.Controllers.Requests.Skill
{
    public class UpdateSkillRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UpdateSkillRequestValidator : AbstractValidator<UpdateSkillRequest>
    {
        public UpdateSkillRequestValidator()
        {
            RuleFor(x => x.Id)
                 .NotEmpty().WithMessage("Empty id");

            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Enter Name")
                 .MaximumLength(20).WithMessage("Max length 20");
        }
    }
}
